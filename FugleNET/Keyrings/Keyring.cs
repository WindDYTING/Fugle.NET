using IniParser;
using IniParser.Model;
using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace FugleNET.Keyrings
{
    public abstract class Keyring
    {
        public const string KEYRING_CRYPTFILE_PASSWORD = "KEYRING_CRYPTFILE_PASSWORD";

        private string _keyringKey;

        protected string Version { get; set; } = "1.0";

        public string FilePath { get; set; }

        public string FileVersion { get; set; }

        public string Scheme { get; set; } = "[PBKDF2] AES256.CFB";

        protected virtual string Decrypt(string passwordEncrypted, byte[] assoc = null)
        {
            return passwordEncrypted;
        }

        protected virtual string Encrypt(string password, byte[] assoc = null)
        {
            return password;
        }


        public string KeyringKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_keyringKey))
                {
                    InitKeyring();
                }

                return _keyringKey;
            }
            set
            {
                if (value.Trim() == string.Empty)
                {
                    throw new Exception("Invalid blank password");
                }

                _keyringKey = value;
                InitKeyring();
            }
        }

        protected abstract void InitKeyring();

#nullable enable
        public string? GetPassword(string service, string username)
        {
            string? password;
            IniData? data = null;
            var config = new FileIniDataParser();
            if (File.Exists(FilePath))
            {
                data = config.ReadFile(FilePath);
            }

            var assoc = GenerateAssoc(service, username);
            service = Utils.Escape(service);
            username = Utils.Escape(username);

            try
            {
                data = Checks.EnsureNotNull(data);
                var passwordBase64 = data[service][username];

                try
                {
                    password = Decrypt(passwordBase64, assoc);
                }
                catch
                {
                    password = Decrypt(passwordBase64);
                }
            }
            catch
            {
                password = null;
            }

            return password;
        }
#nullable disable

        public void SetPassword(string service, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("Username cannot be blank.");
            }

            var config = new FileIniDataParser();
            if (File.Exists(FilePath))
            {
                config.ReadFile(FilePath);
            }

            var assoc = GenerateAssoc(service, username);
            var passwordEncrypted = Encrypt(password, assoc);

            WriteConfigValue(service, username, passwordEncrypted);
        }

        protected void WriteConfigValue(string service, string key, string value)
        {
            if (OperatingSystem.IsWindows())
                EnsureFilePath();
            else if (OperatingSystem.IsLinux())
                EnsureFilePathOnlyLinux();
            else
                throw new PlatformNotSupportedException();

            var config = new FileIniDataParser();
            var configData = config.ReadFile(FilePath);

            service = Utils.Escape(service);
            key = Utils.Escape(key);

            if (!configData.Sections.ContainsSection(service))
            {
                configData.Sections.AddSection(service);
            }

            configData[service].AddKey(key, value);

            using var f = new StreamWriter(File.Open(FilePath, FileMode.Open));
            config.WriteData(f, configData);
        }

        [SupportedOSPlatform("linux")]
        private void EnsureFilePathOnlyLinux()
        {
            const int _600 = Utils.S_IRUSR | Utils.S_IWUSR;
            var storageRoot = Path.GetDirectoryName(FilePath);
            var needStorageRoot = !string.IsNullOrEmpty(storageRoot) && !Directory.Exists(storageRoot);
            if (needStorageRoot)
            {
                Directory.CreateDirectory(storageRoot!);
            }

            if (!File.Exists(FilePath))
            {
                using (_ = File.Open(FilePath, FileMode.Create)) {}

                _ = Utils.chmod(Path.GetFullPath(FilePath), _600);
            }
        }

        [SupportedOSPlatform("windows")]
        private void EnsureFilePath()
        {
            var storageRoot = Path.GetDirectoryName(FilePath);
            var needStorageRoot = !string.IsNullOrEmpty(storageRoot) && !Directory.Exists(storageRoot);
            if (needStorageRoot)
            {
                Directory.CreateDirectory(storageRoot!);
            }

            if (!File.Exists(FilePath))
            {
                using (_ = File.Open(FilePath, FileMode.Create)) {}

                var fSecurity = new FileSecurity(FilePath, AccessControlSections.Owner | AccessControlSections.Access);
                var owner = fSecurity.GetOwner(typeof(NTAccount))!;
                fSecurity.ModifyAccessRule(AccessControlModification.Add,
                    new FileSystemAccessRule(owner, FileSystemRights.Read | FileSystemRights.Write,
                        AccessControlType.Allow),
                    out _);
            }
        }

        private byte[] GenerateAssoc(string service, string username)
        {
            return Encoding.UTF8.GetBytes(Utils.Escape(service) + Utils.Escape(username));
        }
    }
}
