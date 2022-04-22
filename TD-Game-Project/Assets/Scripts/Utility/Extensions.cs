using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
    public static byte[] Compress(byte[] data)
    {
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(output, System.IO.Compression.CompressionLevel.Optimal))
        {
            dstream.Write(data, 0, data.Length);
        }
        return output.ToArray();
    }

    public static byte[] Decompress(byte[] data)
    {
        MemoryStream input = new MemoryStream(data);
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
        {
            dstream.CopyTo(output);
        }
        return output.ToArray();
    }
}
