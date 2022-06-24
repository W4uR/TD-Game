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
    protected static List<Wave> waves;
    public static event Action OnLevelLoaded;
    bool IsEditor => SceneManager.GetActiveScene().name == "Editor";



    public static LevelLoader Singleton { get; private set; }
    public int NumberOfSpawnPoints => tiles.Values.Count(x => x.Type == TileType.Spawner);

    protected virtual void Awake()
    {
        tiles = new Dictionary<HexCoords, Tile>();
        waves = new List<Wave>();
        AddWave();
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
        byte version = levelData[0];
        int numberOfTiles = BitConverter.ToInt32(new byte[] { levelData[1], levelData[2], 0, 0 });
        int pointer = 4;

        //Load tiles
        for (int i = 0; i < numberOfTiles; i++)
        {
            HexCoords coords = new HexCoords(levelData.SubArray(pointer, HexCoords.Size));
            pointer += HexCoords.Size;
            TileType type = (TileType)levelData[pointer++];
            if (type == TileType.Spawner)
            {
                spawnpoints.Add(HexCoords.HexToCartesian(coords)+Vector3.up);
            }
        }

        return spawnpoints;
    }

    public bool LoadLevel(byte[] levelData)
    {
        ClearMap();

        byte version = levelData[0];
        int numberOfTiles = BitConverter.ToInt32(new byte[] { levelData[1], levelData[2], 0, 0 });
        byte numberOfwaves = levelData[3];//Kinda useless for now anyways
        int pointer = 4;

        //Do magic here
        //Load tiles
        for (int i = 0; i < numberOfTiles; i++)
        {
            Tile newTile = Instantiate(tilePrefab);
            HexCoords coords = new HexCoords(levelData.SubArray(pointer, HexCoords.Size));
            pointer += HexCoords.Size;
            TileType type = (TileType)levelData[pointer++];
            newTile.Setup(coords, type);
            newTile.transform.SetParent(transform, true);
            tiles.Add(coords, newTile);
        }
        //Load waves
        while (pointer<levelData.Length) //each cycle is one wave
        {
            Wave wave = new Wave();
            byte numberOfWaveObjects = levelData[pointer++];
            for (byte i = 0; i < numberOfWaveObjects; i++)
            {
                WaveObject waveObject = new WaveObject(levelData[pointer++], levelData[pointer++], levelData[pointer++], levelData[pointer++]);
                wave.AddWaveObject(waveObject);
            }
            waves.Add(wave);
        }
        

        Debug.Log("Loaded level");
        OnLevelLoaded?.Invoke();
        return true;
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

    public static void AddWave()
    {
        waves.Add(new Wave());
    }
    public static void DeleteWave(int waveIndex)
    {
        waves.RemoveAt(waveIndex);
    }
    public static int WaveCount => waves.Count;
}
