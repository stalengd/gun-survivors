using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Data
{
    public static class ListExtensions
    {
        public static T GetOrLast<T>(this IReadOnlyList<T> list, int index)
        {
            if (index >= list.Count)
                index = list.Count - 1;
            return list[index];
        }
    }
}