using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

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
    }
}
