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

    public GameObject brushCellPrefab;
    private List<GameObject> brushCells;

    private List<BrushPreset> brushPresets;
    private Brush brush;
    private int currentBrush = 0;
    protected override void Awake()
    {
        base.Awake();

        brushCells = new List<GameObject>();

        cam = Camera.main;
        LoadBrushes();
        brush = new Brush(brushPresets[currentBrush],0);
    }

    private Vector3 prevpos;
    private void Update()
    {
        if (prevpos!=Input.mousePosition)
        {
            prevpos = Input.mousePosition;
            ShowBrush();
        }
    }
    

    public void ShowBrush()
    {
        foreach (var item in brushCells)
        {
            Destroy(item.gameObject);
        }
        brushCells.Clear();


        HexCoords center = HexCoords.CartesianToHex( cam.ScreenToWorldPoint(Input.mousePosition));

        for (int i = 0; i < brush.Cells; i++)
        {
            HexCoords c = brush.preset.points[i] + center;

            GameObject current = Instantiate(brushCellPrefab, HexCoords.HexToCartesian(c) + Vector3.up, Quaternion.identity);

            brushCells.Add(current);
        }
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
        ShowBrush();
        Debug.Log(currentBrush);
    }


    
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

        RemoveTileAt(HexCoords.CartesianToHex(hitinfo.point.x,hitinfo.point.z));
    }

    void RemoveTileAt(HexCoords center)
    {

        for (int i = 0; i < brush.Cells; i++)
        {

            HexCoords c = brush.preset.points[i] + center;
            if (tiles.ContainsKey(c))
            {
                Destroy(tiles[c].gameObject);
                tiles.Remove(c);
            }
        }

    }
    void CreateTileAt(HexCoords center)
    {


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

        if (Directory.Exists($"{Application.dataPath}/editor/") == false)
        {
            Directory.CreateDirectory($"{Application.dataPath}/editor/");
        }

        int fCount = Directory.GetFiles($"{Application.dataPath}/editor/", "brush_*.tdb", SearchOption.TopDirectoryOnly).Length;
        File.WriteAllBytes($"{Application.dataPath}/editor/brush_{fCount}.tdb", Extensions.Compress(bytes));

        Debug.Log("Saved Brush");
        LoadBrushes();
    }
    
    public void LoadBrushes()
    {
        brushPresets = new List<BrushPreset>();
        brushPresets.Add(new BrushPreset());

        string[] brushes = Directory.GetFiles($"{Application.dataPath}/editor/", "brush_*.tdb", SearchOption.AllDirectories);

        foreach (var path in brushes)
        {
            byte[] bytes = Extensions.Decompress(File.ReadAllBytes(path));
            brushPresets.Add(new BrushPreset(bytes));
        }
        Debug.Log("Loaded Brushes");
    }

}
