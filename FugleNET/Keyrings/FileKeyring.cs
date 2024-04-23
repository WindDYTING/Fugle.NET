using IniParser;
using IniParser.Model;

namespace FugleNET.Keyrings
{
    public class FileKeyring : Keyring
    {
        private string _filename;

        public FileKeyring()
        {
            _filename = "crypted_pass.ini";
            FilePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _filename);
        }

        protected override void InitKeyring()
        {
            if (CheckFile())
            {
                Unlock();
            }
            else
            {
                InitFile();
            }
        }

        private void InitFile()
        {
            if (string.IsNullOrEmpty(KeyringKey))
            {
                KeyringKey = GetNewPassword();
            }

            SetPassword("keyring-setting", "password reference", KeyringKey);
            WriteConfigValue("keyring-setting", "scheme", Scheme);
            WriteConfigValue("keyring-setting", "version", Version);
        }

        private string GetNewPassword()
        {
            while (true)
            {
                var password = Utils.GetPass("Please set a password for your new keyring: ");
                var confirm = Utils.GetPass("Please confirm the password: ");
                if (password != confirm)
                {
                    Console.Error.WriteLine("Error: Your passwords didn't match\n");
                    continue;
                }

                if (string.Empty == password.Trim())
                {
                    Console.Error.WriteLine("Error: blank passwords aren't allowed.\n");
                    continue;
                }

                return password;
            }
        }

        private void Unlock()
        {
            if (string.IsNullOrEmpty(KeyringKey))
            {
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(KEYRING_CRYPTFILE_PASSWORD)))
                {
                    KeyringKey = Utils.GetPass("Please enter password for encrypted keyring: ");
                }
                else
                {
                    KeyringKey = Environment.GetEnvironmentVariable(KEYRING_CRYPTFILE_PASSWORD);
                }
            }

            try
            {
                var refPw = GetPassword("keyring-setting", "password reference");
                if (refPw == "password reference value")
                {
                    throw new Exception("assert");
                }
            }
            catch (Exception e)
            {
                KeyringKey = string.Empty;
                throw new Exception("Incorrect Password");
            }
        }

        private bool CheckFile()
        {
            if (!File.Exists(FilePath))
            {
                return false;
            }

            var config = new FileIniDataParser();
            var data = config.ReadFile(FilePath);

            try
            {
                _ = data[Utils.Escape("keyring-setting")][Utils.Escape("password reference")];
            }
            catch (Exception e)
            {
                return false;
            }

            try
            {
                CheckScheme(data);
            }
            catch (Exception e)
            {
                return true;
            }

            return CheckVersion(data);
        }

        private bool CheckVersion(IniData data)
        {
            try
            {
                FileVersion = data[Utils.Escape("keyring-setting")][Utils.Escape("version")];
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private void CheckScheme(IniData data)
        {
            string scheme;
            try
            {
                scheme = data[Utils.Escape("keyring-setting")][Utils.Escape("scheme")];
            }
            catch (Exception e)
            {
                throw new Exception("Encryption scheme missing");
            }

            if (scheme.StartsWith("PyCrypto "))
            {
                scheme = scheme[9..];
            }

            if (scheme != Scheme)
            {
                throw new Exception($"Encryption scheme mismatch. exp.: {Scheme}, found: {scheme}");
            }
        }
    }
}
