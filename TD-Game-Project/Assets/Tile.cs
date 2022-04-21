using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    HexCoords coords;
    public TMP_Text text;
    public void Setup(float x, float y)
    {
        coords = HexCoords.CartesianToHex(x, y);
        

        


    }

    public void SetHex(int q, int r)
    {
        coords = new HexCoords(q, r);
        transform.position = HexCoords.HexToCartesian(coords);
        text.text = $"{coords.Q}:{coords.R}";
    }

}
