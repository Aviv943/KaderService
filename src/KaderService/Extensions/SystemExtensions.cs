using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KaderService.Extensions
{
    public static class SystemExtensions
    {
        public static void AddToFront<T>(this List<T> list, T item)
        {
            // omits validation, etc.
            list.Insert(0, item);
        }
    }
}
