using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;

public static class Extensions
{


    private static Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => isoMatrix.MultiplyPoint3x4(input);
    public static Vector3 To3D(this Vector2 vector) => new Vector3(vector.x, 0, vector.y);
    public static Vector3 ToIso(this Vector2 input) => input.To3D().ToIso();
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

    public static T GetRandomElement<T>(this IEnumerable<T> container)
    {
        var rnd = new System.Random();
        return container.OrderBy(x => rnd.Next()).FirstOrDefault();
    }

    public static T[] To1DArray<T>( this T[][] matrix)
    {
        T[] array = new T[matrix.Length];
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = matrix[i%w][i/w];
        }
        return array;
    }

}
