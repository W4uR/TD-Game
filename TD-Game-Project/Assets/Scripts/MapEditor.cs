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


    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
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

        CreateTileAt(hitinfo.point, 0);
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
    void CreateTileAt(Vector3 pos, byte type)
    {

        Tile current = Instantiate(tilePrefab, pos, Quaternion.identity);
        HexCoords coords = HexCoords.CartesianToHex(pos.x, pos.z);
        current.Setup(coords,type);



        current.transform.SetParent(TilesParent, true);

        if (tiles.ContainsKey(coords))
        {
            if (tiles[coords].Type == current.Type)
            {
                Destroy(current.gameObject);
                return;
            }
            else
            {
                tiles.Remove(coords);
            }
        }
        tiles.Add(coords, current);
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
    
}
