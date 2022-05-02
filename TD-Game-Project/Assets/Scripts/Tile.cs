using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    byte type = 0;

    public TMP_Text text;

    public byte Type { get => type;}

    public void Setup(HexCoords coords, byte type)
    {
        transform.position = HexCoords.HexToCartesian(coords);// + Vector3.up * transform.position.y;
        text.text = $"{coords}\n{type}";
        name = $"Tile {coords}";
        this.type = type;
        GetComponent<MeshRenderer>().material.color = Type == 0 ? Color.green : Color.blue;
    }

}
