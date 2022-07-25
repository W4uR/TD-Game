using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class LevelEditor : LevelLoader
{

    public Camera Cam;

    [SerializeField]
    BrushSelector brushSelector;

    private bool paintEmptyOnly;
    private bool isCursor = false;


    public static LevelEditor Instance;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null) Instance = this;
        Cam = Camera.main;
    }
    private void OnEnable()
    {
        LE_InputManager.LeftMouseButton += HandleLeftMouseButton;
    }
    private void OnDisable()
    {
        LE_InputManager.LeftMouseButton -= HandleLeftMouseButton;
    }
    public void SaveLevel(string levelName)
    {


        byte[] bytes = new byte[4+(Tile.Size * tiles.Count)+(WaveEditor.Waves.Sum(x=>x.Size)+ WaveEditor.Waves.Count)];

        bytes[0] = 0;//Version
        bytes[1] = BitConverter.GetBytes(tiles.Count)[0];//lower byte
        bytes[2] = BitConverter.GetBytes(tiles.Count)[1];//higher byte
        bytes[3] = (byte)WaveEditor.Waves.Count;

        //Write Tiles
        int pointer = 4;
        foreach (var tile in tiles)
        {
            byte[] qrCoords = tile.Key.ToBytes();
            for (int i = 0; i < HexCoords.Size; i++)
            {
                bytes[pointer + i] = qrCoords[i];
            }
            bytes[pointer + HexCoords.Size] = (byte)tile.Value.Type;

            pointer += Tile.Size;
        }
        //Write Waves
        foreach (var wave in WaveEditor.Waves)
        {
            byte waveSize = wave.NumberOfWaveObjects;
            byte[] waveBytes = wave.ToBytes;
            bytes[pointer++] = waveSize;
            for (int i = 0; i < waveBytes.Length; i++)
            {
                bytes[pointer++] = waveBytes[i];
            }
        }
        if (pointer != bytes.Length)
        {
            Debug.LogError("ITT A BAJ");
        }

        if (!Directory.Exists($"{Application.dataPath}/levels")) Directory.CreateDirectory($"{Application.dataPath}/levels");
        File.WriteAllBytes($"{Application.dataPath}/levels/{levelName}.td", Extensions.Compress(bytes));

        Debug.Log("Saved level: " + levelName);
    }   
    private void Update()
    {
        if (!isCursor)
        {
            brushSelector.ShowPreview(HexCoords.CartesianToHex(Cam.ScreenToWorldPoint(Input.mousePosition)));
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Casts the ray and get the first game object hit
            Physics.Raycast(ray, out hit);
            if (hit.collider.TryGetComponent<Tile>(out Tile tile))
            {
                if (tile.Type == TileType.EnemySpawner)
                {

                    //Clicked on enemyspawner --- Open spawner editor, set selected spawner
                    byte index = 0;
                    var spawners = tiles.Values.Where(x => x.Type == TileType.EnemySpawner).ToArray();
                    while (index < spawners.Count())
                    {
                        if (spawners[index].Coords == HexCoords.CartesianToHex(hit.point))
                        {
                            //Found the one
                            break;
                        }
                        index++;
                    }
                    WaveEditor.SelectedSpawner = index;
                    WaveEditor.Singleton.OpenWindow();
                    Debug.Log("SELECTED SPAWNER IS: " + index);
                    Debug.Log("OPEN SPAWNER EDITOR");

                }
            }
        }
    }
    public void HandleLeftMouseButton()
    {

        if (LE_InputManager.MouseOverUI) return;
        if (isCursor) return;

        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hitinfo);

        HexCoords center = HexCoords.CartesianToHex(hitinfo.point.x, hitinfo.point.z);

        if (brushSelector.IsEreaser)
        {
            EreaseAt(center);
        }
        else
        {
            PaintAt(center);
        }

    }
    void EreaseAt(HexCoords center)
    {

        foreach (HexCoords direction in brushSelector.SelectedBrush.GetCells())
        {
            HexCoords coord = direction + center;
            if (tiles.ContainsKey(coord))
            {
                RemoveTileAt(coord);
            }
        }

    }
    void PaintAt(HexCoords center)
    {

        foreach (HexCoords offset in brushSelector.SelectedBrush.GetCells())
        {
            HexCoords coord = center + offset;

            Tile current = CreateTile(coord);


            if (tiles.ContainsKey(coord))
            {
                    
                if (tiles[coord].Type == current.Type || paintEmptyOnly)
                {
                    Destroy(current.gameObject);
                    continue;
                }
                else
                {
                    RemoveTileAt(coord);
                }
            }

                

            tiles.Add(coord, current);
        }

    }
    public void RemoveTileAt(HexCoords coords)
    {
        Destroy(tiles[coords].gameObject);
        tiles.Remove(coords);
    }
    private Tile CreateTile(HexCoords coord)
    {
        Tile current = Instantiate(tilePrefab, HexCoords.HexToCartesian(coord), Quaternion.identity, transform);

        current.Setup(coord, brushSelector.SelectedType);

        return current;
    }
    public void TogglePaintEmpty()
    {
        paintEmptyOnly = !paintEmptyOnly;
    }
    public void ToggleCursor()
    {
        isCursor = !isCursor;
    }
}

