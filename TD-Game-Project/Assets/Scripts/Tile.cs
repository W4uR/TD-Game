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
    //----
    public TMP_Text text;

    public TileType Type { get => type;}
    public HexCoords Coords => HexCoords.CartesianToHex(transform.position);

    public void Setup(HexCoords coords, TileType type)
    {
        transform.position = HexCoords.HexToCartesian(coords);// + Vector3.up * transform.position.y;
        text.text = $"{coords}\n{type}";
        name = $"Tile {coords}";
        this.type = type;
        GetComponent<MeshRenderer>().material.color = Type == 0 ? Color.green : Color.blue;
        
    }

    public static int Size => HexCoords.Size + 1;
}
