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
    /*
    public static List<byte> ToByteList(this int number)
    {

    }
    */
}
