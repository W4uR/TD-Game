using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush
{
    public BrushPreset preset;
    int type = 0;

    public Brush(BrushPreset _preset, int _type)
    {
        preset = _preset;
        type = _type;
    }
    public Brush()
    {
        preset = new BrushPreset();
    }

    public int Type { get => type; set => type = value; }
    public int Cells { get => preset.points.Count; }
}

public class BrushPreset
{
    public List<HexCoords> points;
    public BrushPreset(byte[] bytes)
    {
        points = new List<HexCoords>();
        for (int offset = 0; offset < bytes.Length; offset+=8)
        {
            points.Add(new HexCoords(bytes.SubArray(offset, 8)));
        }
    }

    public BrushPreset()
    {
        points = new List<HexCoords>();
        points.Add(new HexCoords(0, 0));
    }
}
