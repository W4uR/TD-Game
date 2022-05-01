using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Transform TilesParent;

    [SerializeField]
    protected Tile tilePrefab;

    public Dictionary<HexCoords, Tile> tiles;

    private NetworkManagerTDGame room;
    private NetworkManagerTDGame Room
    {
        get { if (room != null) return room; return room = NetworkManager.singleton as NetworkManagerTDGame; }
    }




    protected virtual void Awake()
    {
        tiles = new Dictionary<HexCoords, Tile>();
    }


    public void LoadLevel(byte[] levelData)
    {
        ClearMap();
        for (int offset = 0; offset < levelData.Length; offset += 9)
        {
            Tile current = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);

            HexCoords coords = new HexCoords(levelData.SubArray(offset, 8));
            byte type = levelData[offset + 8];
            current.Setup(coords, type);
            current.transform.SetParent(TilesParent, true);
            tiles.Add(coords, current);

            if (type == 1) //spawner tile
            {

            }

        }
        Debug.Log("Loaded");
        // Spawn player
    }

    public void LoadLevel(string levelName)
    {     
        if (!Directory.Exists($"{Application.dataPath}/levels")) Directory.CreateDirectory($"{Application.dataPath}/levels");
        byte[] bytes = Extensions.Decompress(File.ReadAllBytes($"{Application.dataPath}/levels/{levelName}.td"));

        LoadLevel(bytes);   
    }

    protected void ClearMap()
    {
        foreach (var tile in tiles)
        {
            Destroy(tile.Value.gameObject);
        }
        tiles.Clear();
    }
}
