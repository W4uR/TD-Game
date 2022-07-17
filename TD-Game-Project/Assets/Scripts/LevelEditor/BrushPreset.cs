using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class BrushPreset
{
    public List<HexCoords> points;
    public BrushPreset(byte[] bytes)
    {
        points = new List<HexCoords>();
        for (int offset = 0; offset < bytes.Length; offset += 8)
        {
            points.Add(new HexCoords(bytes.SubArray(offset, 8)));
        }
    }
    public BrushPreset(List<HexCoords> coords)
    {
        points = coords;
    }
    public BrushPreset()
    {
        points = new List<HexCoords>();
        points.Add(new HexCoords(0, 0));
    }


    public IEnumerable<HexCoords> GetCells()
    {
        // Iterating the elements of myarray
        foreach (HexCoords point in points)
        {
            // Returning the element after every iteration
            yield return point;
        }
    }

}
