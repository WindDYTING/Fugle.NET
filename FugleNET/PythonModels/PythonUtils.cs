using System.Collections.Generic;
using System.Reflection;

namespace FugleNET.PythonModels
{
    internal static class PythonUtils
    {
        public static Dictionary<string, dynamic> Items<T>(this T input)
        {
            var fields = typeof(T).GetFields();
            var dict = new Dictionary<string, dynamic>();

            foreach (var info in fields)
            {
                dict[info.Name] = info.GetValue(input);
            }

            return dict;
        }
    }
}
