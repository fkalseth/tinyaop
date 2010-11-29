using System;
using System.Collections.Generic;

namespace TinyAop
{
    public static class EnumerableExt
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration) action(item);
        }
    }
}