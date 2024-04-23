using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using IniParser;
using IniParser.Model;

namespace FugleNET.Keyrings
{
    public abstract class Keyring
    {
        public const string KEYRING_CRYPTFILE_PASSWORD = "KEYRING_CRYPTFILE_PASSWORD";

        private string _keyringKey;

        protected string Version { get; set; } = "1.0";

        public string FilePath { get; set; }

        public string FileVersion { get; set; }

        public string Scheme { get; set; }

        protected virtual byte[] Decrypt(byte[] passwordEncrypted, byte[]? assoc = null)
        {
            return passwordEncrypted;
        }

        protected virtual byte[] Encrypt(string password, byte[]? assoc = null)
        {
            return Encoding.ASCII.GetBytes(password);
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
                var passwordBase64 = Convert.FromBase64String(data[service][username]);
              //  var passwordEncrypted = Encoding.UTF8.GetBytes(passwordBase64);

                try
                {
                    password = Encoding.UTF8.GetString(Decrypt(passwordBase64, assoc));
                }
                catch (Exception e)
                {
                    password = Encoding.UTF8.GetString(Decrypt(passwordBase64));
                }
            }
            catch
            {
                password = null;
            }

            return password;
        }

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
            var passwordBase64 = Convert.ToBase64String(passwordEncrypted);

            WriteConfigValue(service, username, passwordBase64);
        }

        protected void WriteConfigValue(string service, string key, string value)
        {
            EnsureFilePath();

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

        private void EnsureFilePath()
        {
            var storageRoot = Path.GetDirectoryName(FilePath);
            var needStorageRoot = !string.IsNullOrEmpty(storageRoot) && File.GetAttributes(storageRoot) != FileAttributes.Directory;
            if (needStorageRoot)
            {
                Directory.CreateDirectory(storageRoot!);
            }

            if (!File.Exists(FilePath))
            {
                using (_ = File.Open(FilePath, FileMode.Create)) {}

                var fSecurity = new FileSecurity(FilePath, AccessControlSections.Owner | AccessControlSections.Access);
                var owner = fSecurity.GetOwner(typeof(NTAccount));
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
