using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class Brush
{
    private int currentPreset = 0;
    private BrushPreset[] presets;
    byte type = 0;

    private List<GameObject> previewCells;


    bool isEreaser;
    public byte Type { get => type;}
    public bool IsEreaser { get => isEreaser;}



    ~Brush(){
        InputManager.RightMouseButton -= ScrollType;
        InputManager.E_Button -= ToggleEreaser;
        InputManager.MouseWheel -= ScrollBrush;
        InputManager.T_Button -= ScrollType;
    }
    public Brush(string[] _preset_paths, byte _type, bool _isEreaser)
    {
        presets = new BrushPreset[_preset_paths.Length+1];
        presets[0] = new BrushPreset();
        previewCells = new List<GameObject>();
        type = _type;
        isEreaser = _isEreaser;

        for (int i = 1; i < presets.Length; i++)
        {
            byte[] bytes = Extensions.Decompress(File.ReadAllBytes(_preset_paths[i-1]));
            presets[i] = new BrushPreset(bytes);
        }

        InputManager.RightMouseButton += ScrollType;
        InputManager.E_Button += ToggleEreaser;
        InputManager.MouseWheel += ScrollBrush;
        InputManager.T_Button += ScrollType;
    }

    public void Clear()
    {
        foreach (var previewCell in previewCells)
        {
            GameObject.Destroy(previewCell);
        }
        previewCells.Clear();
    }

    private void ToggleEreaser()
    {
        isEreaser = !isEreaser;
        UIManager.Instance.SetIsEreaser(IsEreaser);
    }

    
    public void Show(HexCoords center)
    {
        Clear();


        foreach (HexCoords direction in GetCells())
        {
            HexCoords coord = direction + center;

            GameObject current = GameObject.Instantiate(MapEditor.Instance.brushCellPrefab, HexCoords.HexToCartesian(coord) + Vector3.up, Quaternion.identity);
            previewCells.Add(current);
        }

    }
    
    public void ScrollBrush(bool toRight)
    {
        if (Input.GetKey(KeyCode.LeftControl)) return;

        if (toRight)
        {
            currentPreset = ++currentPreset % presets.Length;
        }
        else
        {
            currentPreset--;
            if (currentPreset < 0)
            {
                currentPreset = presets.Length - 1;
            }
        }

    }

    public void ScrollType()
    {
        type = (byte)(++type % 2);
    }


    public IEnumerable<HexCoords> GetCells()
    {
        return presets[currentPreset].GetCells();
    }
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
