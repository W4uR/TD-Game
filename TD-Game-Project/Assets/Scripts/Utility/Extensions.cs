using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T[] SubArray<T>(this T[] array, int offset, int length)
    {
        return new List<T>(array)
                    .GetRange(offset, length)
                    .ToArray();
    }
    
    public static T[] Concat<T>(T[] array1, T[] array2)
    {
        List<T> l = new List<T>(array1);
        l.AddRange(array2);
        return l.ToArray();
    }
    
}
