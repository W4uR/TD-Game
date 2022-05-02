using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mirror;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Transform TilesParent;

    [SerializeField]
    protected Tile tilePrefab;

    protected static Dictionary<HexCoords, Tile> tiles;

    bool addCollider = false;

    private NetworkManagerTDGame room;
    public static LevelLoader Singleton { get; private set; }
    private NetworkManagerTDGame Room
    {
        get { if (room != null) return room; return room = NetworkManager.singleton as NetworkManagerTDGame; }
    }

    public Vector3? GetRandomSpawnPoint()
    {
        var rnd = new System.Random();
        var spawnTile = tiles.Values.Where(x => x.Type == 1).OrderBy(x => rnd.Next()).FirstOrDefault();
        if (spawnTile == null)
        {
            return null;
        }
        return spawnTile.transform.position+Vector3.up*2f;
    }


    protected virtual void Awake()
    {
        tiles = new Dictionary<HexCoords, Tile>();
        if (Singleton == null)
            Singleton = this;
        else
        {
            Debug.LogError("There can be only one LevelLoader");
        }
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
            
            current.GetComponent<MeshCollider>().enabled = addCollider;
        }
        Debug.Log("Loaded");
    }

    internal void AddColliders()
    {
        addCollider = true;
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
