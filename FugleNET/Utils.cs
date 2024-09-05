using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

#nullable enable

namespace FugleNET
{
    internal static class Utils
    {
        private const string LEGAL_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }

        public static Dictionary<string, object> FromJson(dynamic obj)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(obj);
        }

        public static T? FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        

        public static string Escape(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return string.Join("", bytes.Select(EscapeChar));
        }

        internal static string EscapeChar(byte c)
        {
            char ch = (char)c;
            if (LEGAL_CHARS.Contains(ch))
            {
                return ch.ToString();
            }

            return $"_{(byte)ch:02X}";
        }

        public static string GetPass(string prompt = "Password:")
        {
            var password = new StringBuilder();
            Console.Write(prompt);
            do
            {
                var key = Console.ReadKey(true).KeyChar;

                if (key == '\b')
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else if (key == '\r')
                {
                    Console.WriteLine();
                    break;
                }
                else if (!char.IsControl(key))
                {
                    password.Append(key);
                    Console.Write('*');
                }
            } while (true);

            return password.ToString();
        }

         
        [DllImport("libc", SetLastError = true)]
        internal static extern int chmod(string pathname, int mode);

        // user permissions
        internal const int S_IRUSR = 0x100;
        internal const int S_IWUSR = 0x80;
        internal const int S_IXUSR = 0x40;
         
        // group permission
        internal const int S_IRGRP = 0x20;
        internal const int S_IWGRP = 0x10;
        internal const int S_IXGRP = 0x8;
         
        // other permissions
        internal const int S_IROTH = 0x4;
        internal const int S_IWOTH = 0x2;
        internal const int S_IXOTH = 0x1;
    }
}
