using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Transform TilesParent;

    [SerializeField]
    protected Tile tilePrefab;

    public Dictionary<HexCoords, Tile> tiles;

    protected virtual void Awake()
    {
        tiles = new Dictionary<HexCoords, Tile>();
    }

    public virtual void Save()
    {

    }

    public void LoadLevel(string levelName)
    {
        ClearMap();
        if (!Directory.Exists($"{Application.dataPath}/levels")) Directory.CreateDirectory($"{Application.dataPath}/levels");
        byte[] bytes = Extensions.Decompress(File.ReadAllBytes($"{Application.dataPath}/levels/{levelName}.td"));


        for (int offset = 0; offset < bytes.Length; offset += 9)
        {
            Tile current = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);

            HexCoords coords = new HexCoords(bytes.SubArray(offset, 8));
            byte type = bytes[offset + 8];
            current.Setup(coords, type);
            current.transform.SetParent(TilesParent, true);
            tiles.Add(coords, current);
        }

        Debug.Log("Loaded");
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
