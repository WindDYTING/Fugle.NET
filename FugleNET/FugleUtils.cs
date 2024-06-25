using System;
using System.Security.Cryptography;
using System.Text;
using FugleNET.Keyrings;

namespace FugleNET
{
    public static class FugleUtils
    {
        internal static Keyring Kr { get; set; }

        public static void SetupKeyring(string userAccount)
        {
            Kr = new CryptFileKeyring()
            {
                KeyringKey = Environment.GetEnvironmentVariable("KEYRING_CRYPTFILE_PASSWORD") ??
                            HashValue(userAccount)
            };
        }

        public static string HashValue(string val)
        {
            var data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(val));
            return Convert.ToHexString(data);
        }

        public static string? GetPassword(string key, string userAccount)
        {
            return Kr.GetPassword(key, userAccount);
        }

        public static void CheckPassword(string userAccount)
        {
            if (string.IsNullOrEmpty(GetPassword("fugle_trade_sdk:account", userAccount)))
            {
                Kr.SetPassword("fugle_trade_sdk:account", userAccount, Utils.GetPass("Enter esun account password:\n"));
            }

            if (string.IsNullOrEmpty(GetPassword("fugle_trade_sdk:cert", userAccount)))
            {
                Kr.SetPassword("fugle_trade_sdk:cert", userAccount, Utils.GetPass("Enter cert password:\n"));
            }
        }

        public static void SetPassword(string userAccount)
        {
            Kr.SetPassword("fugle_trade_sdk:account", userAccount, Utils.GetPass("Enter esun account password:\n"));
            Kr.SetPassword("fugle_trade_sdk:cert", userAccount, Utils.GetPass("Enter cert password:\n"));
        }
    }
}
