using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FugleNET
{
    internal class Checks
    {
        [return: NotNull]
        public static T EnsureNotNull<T>(T obj)
        {
            ThrowIsNull(obj);
            return obj;
        }

        public static void ThrowIsNull<T>(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
        }

        public static void ThrowIsEmpty<T>(IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (!source.Any()) throw new ArgumentException($"Array {nameof(source)} is empty", nameof(source));
        }

        public static void ThrowIsBlank(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) throw new ArgumentNullException(nameof(str));
        }
    }
}
