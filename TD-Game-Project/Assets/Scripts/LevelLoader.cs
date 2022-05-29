using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    protected Tile tilePrefab;

    protected static Dictionary<HexCoords, Tile> tiles;
    public static event Action OnLevelLoaded;
    bool IsEditor => SceneManager.GetActiveScene().name == "Editor";



    public static LevelLoader Singleton { get; private set; }
    public static List<Vector3> SpawnPoints = new List<Vector3>();

    public static Vector3 GetRandomSpawnPoint()
    {
        var rnd = new System.Random();
        return SpawnPoints.OrderBy(x => rnd.Next()).FirstOrDefault();
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
    private void OnDestroy()
    {
        ClearMap();
    }
    public static List<Vector3> SpawnPointsFromData(byte[] levelData)
    {
        List<Vector3> spawnpoints = new List<Vector3>();
        for (int offset = 0; offset < levelData.Length; offset += 9)
        {
            HexCoords coords = new HexCoords(levelData.SubArray(offset, 8));
            byte type = levelData[offset + 8];

            if (type == 1)
            {
                spawnpoints.Add(HexCoords.HexToCartesian(coords)+Vector3.up);
            }

        }
        return spawnpoints;
    }

    public bool LoadLevel(byte[] levelData)
    {
        ClearMap();
        for (int offset = 0; offset < levelData.Length; offset += 9)
        {
            Tile current = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);

            HexCoords coords = new HexCoords(levelData.SubArray(offset, 8));
            byte type = levelData[offset + 8];
            current.Setup(coords, type);
            current.transform.SetParent(transform, true);
            tiles.Add(coords, current);

            current.GetComponent<MeshCollider>().enabled = !IsEditor;
            if (type == 1)
            {
                SpawnPoints.Add(current.transform.position + Vector3.up*2f);
            }
            
        }
        Debug.Log("Loaded");
        OnLevelLoaded?.Invoke();
        return true;
    }
    /*
    private void CombineMeshes()
    {
        var combine = new CombineInstance[tiles.Count];
        var tileMeshes = tiles.Values.Select(x => x.GetComponent<MeshFilter>()).ToArray();
        for (int i = 0; i < tileMeshes.Length; i++)
        {
            combine[i].mesh = tileMeshes[i].mesh;
            combine[i].transform = tileMeshes[i].transform.localToWorldMatrix;
        }
        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = tiles.First().Value.GetComponent<MeshRenderer>().material;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().enabled = true;
        foreach (var tile in tiles)
        {
            tile.Value.gameObject.SetActive(false);
        }
    }
    */
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
        SpawnPoints.Clear();
    }
}
