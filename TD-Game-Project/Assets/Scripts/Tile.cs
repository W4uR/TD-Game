using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TileType
{
    Plain,
    Forest,
    Quarry,
    Spawner,
    EnemySpawner,
    Nexus,
    Water
}

public class Tile : MonoBehaviour
{
    TileType type = TileType.Plain;

    //extras
    public bool Walkable => type == TileType.Plain || type == TileType.Spawner || type == TileType.EnemySpawner;
    public TileType Type { get => type;}
    public HexCoords Coords => HexCoords.CartesianToHex(transform.position);
    //----

    public void Setup(HexCoords coords, TileType type)
    {
        transform.position = HexCoords.HexToCartesian(coords);// + Vector3.up * transform.position.y;
        name = $"Tile {coords}";
        this.type = type;
        //GetComponent<MeshRenderer>().material.color = Type == 0 ? Color.green : Color.blue;

        Color c = Color.green;
        switch (type)
        {
            case TileType.Forest: c = new Color(0f, 0.16f, 0.1f); break;
            case TileType.Quarry: c = Color.gray; break;
            case TileType.Spawner: c = Color.yellow; break;
            case TileType.EnemySpawner: c = Color.red; break;
            case TileType.Nexus: c = Color.magenta; break;
            case TileType.Water: c = Color.blue; break;

            case TileType.Plain:
            default:
                break;
        }
        GetComponent<MeshRenderer>().material.color = c;

    }

    // File management
    public static int Size => HexCoords.Size + 1;
}
