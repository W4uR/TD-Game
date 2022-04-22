using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.IO.Compression;

public class MapEditor : LevelLoader
{
    

    private Camera cam;

    private List<BrushPreset> brushPresets;
    private Brush brush;
    private int currentBrush = 0;
    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
        LoadBrushes();
        brush = new Brush(brushPresets[currentBrush],0);
    }

    public void ScrollBrush(bool toRight)
    {
        if (toRight)
        {
            currentBrush = ++currentBrush % brushPresets.Count;
        }
        else
        {
            currentBrush--;
            if (currentBrush < 0)
            {
                currentBrush = brushPresets.Count - 1;
            }
        }
        brush = new Brush(brushPresets[currentBrush], 0);
        Debug.Log(currentBrush);
    }


    /*
    private void Start()
    {
        
        for (int i = -10; i < 10; i++)
        {
            for (int j = -6; j < 6; j++)
            {
                GameObject go = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
                Tile current = go.GetComponent<Tile>();
                current.SetHex(i,j);

                tiles.Add(current);
            }
        }
        
    }
    */
    
    public void OnLeftMouseBtn()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hitinfo);

        CreateTileAt(HexCoords.CartesianToHex(hitinfo.point.x,hitinfo.point.z));
    }

    public void OnRightMouseBtn()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hitinfo);

        RemoveTileAt(hitinfo.point);
    }

    void RemoveTileAt(Vector3 pos)
    {
        HexCoords coords = HexCoords.CartesianToHex(pos.x, pos.z);
        if (tiles.ContainsKey(coords))
        {
            Destroy(tiles[coords].gameObject);
            tiles.Remove(coords);
        }
    }
    void CreateTileAt(HexCoords center)
    {

        if (brushPresets.Count == 0)
        {
            Tile current = Instantiate(tilePrefab, HexCoords.HexToCartesian(center), Quaternion.identity);

            current.Setup(center, 0);

            current.transform.SetParent(TilesParent, true);

            if (tiles.ContainsKey(center))
            {
                if (tiles[center].Type == current.Type)
                {
                    Destroy(current.gameObject);
                    return;
                }
                else
                {
                    tiles.Remove(center);
                }
            }
            tiles.Add(center, current);
        }

        for (int i = 0; i < brush.Cells; i++)
        {
            HexCoords c =  brush.preset.points[i] + center;

            Tile current = Instantiate(tilePrefab, HexCoords.HexToCartesian(c), Quaternion.identity);

            current.Setup(c, 0);

            current.transform.SetParent(TilesParent, true);

            if (tiles.ContainsKey(c))
            {
                if (tiles[c].Type == current.Type)
                {
                    Destroy(current.gameObject);
                    continue;
                }
                else
                {
                    tiles.Remove(center);
                }
            }
            tiles.Add(c, current);

        }
        
    }


    public override void Save()
    {

        byte[] bytes = new byte[9*tiles.Count];

        int offset = 0;
        foreach (var tile in tiles)
        {
            byte[] qrCoords = tile.Key.ToBytes();
            for (int i = 0; i < 8; i++)
            {
                bytes[offset + i] = qrCoords[i];
            }
            bytes[offset + 8] = tile.Value.Type;

            offset+=9; // 4 + 4 bytes are the coords and 1 byte is the type
        }

        File.WriteAllBytes(Application.dataPath + "/map01.td", Extensions.Compress(bytes));

        Debug.Log("Saved");
    }

    public void SaveBrush()
    {
        byte[] bytes = new byte[8 * tiles.Count];

        int offset = 0;
        foreach (var tile in tiles)
        {
            byte[] qrCoords = tile.Key.ToBytes();
            for (int i = 0; i < 8; i++)
            {
                bytes[offset + i] = qrCoords[i];
            }

            offset += 8;
        }
        int fCount = Directory.GetFiles($"{Application.dataPath}/editor/", "*", SearchOption.TopDirectoryOnly).Length;
        File.WriteAllBytes($"{Application.dataPath}/editor/brush_{fCount}.tdb", Extensions.Compress(bytes));

        Debug.Log("Saved Brush");
    }
    
    public void LoadBrushes()
    {
        brushPresets = new List<BrushPreset>();

        int brushCount = Directory.GetFiles($"{Application.dataPath}/editor/", "brush_*.tdb", SearchOption.TopDirectoryOnly).Length;
        for (int i = 0; i < brushCount; i++)
        {
            byte[] bytes = Extensions.Decompress(File.ReadAllBytes($"{Application.dataPath}/editor/brush_{2*i}.tdb"));
            brushPresets.Add(new BrushPreset(bytes));
        }

    }

}
