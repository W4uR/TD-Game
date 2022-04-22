using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoords
{
    int q, r;


    static readonly private float size = 0.5f;
    static readonly private float width = 2f * size;
    static readonly private float sqrt_3 = Mathf.Sqrt(3f);
    static readonly private float height = sqrt_3 * size;

    public int Q { get => q;}
    public int R { get => r;}
    public int S { get => -q-r; }

    public HexCoords(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public HexCoords(Vector3Int cubeCoords)
    {
        q = cubeCoords.x;
        r = cubeCoords.y;
    }

    /*
    public HexCoords(byte[] bytes)
    {
        // Q
        for (int i = 0; i < 4; i++)
        {
            q = q << (i * 8);

            q +=bytes[i];
        }
        // R
        for (int i = 4; i < 8; i++)
        {
            r = r << (i * 8);

            r += bytes[i];
        }
    }
    */
    public static Vector3 HexToCartesian(HexCoords coords)
    {
        float x = (3f *.5f * coords.Q) * size;
        float y = (sqrt_3 * .5f * coords.Q + sqrt_3 * coords.R) * size;
        return new Vector3(x , 0, y);
    }


    public static HexCoords CartesianToHex(float x, float y)
    {
        //Do magic here
        float newQ = (2f / 3f) * x / size;
        float newR = ((-1f / 3f) * x + (sqrt_3 / 3f) * y) / size;

        HexCoords coords = new HexCoords(RoundCoords(newQ, newR));

        return coords;
    }



    static Vector3Int RoundCoords(float _q, float _r)
    {
        var _s = (-_q - _r);

        int _Q = (int)Mathf.Round(_q);
        int _R = (int)Mathf.Round(_r);
        int _S = (int)Mathf.Round(_s);

        var q_diff = Mathf.Abs(_Q - _q);
        var r_diff = Mathf.Abs(_R - _r);
        var s_diff = Mathf.Abs(_S - _s);

        if (q_diff > r_diff && q_diff > s_diff)
            _Q = -_R - _S;
        else if (r_diff > s_diff)
            _R = -_Q - _S;
        else
            _S = -_Q - _R;

        return new Vector3Int(_Q, _R, _S);
    }

    public override bool Equals(object obj)
    {
        return obj is HexCoords coords &&
               q == coords.q &&
               r == coords.r;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(q, r);
    }

    public static bool operator ==(HexCoords left, HexCoords right)
    {
        return left.Q == right.Q && left.R == right.R;
    }
    public static bool operator !=(HexCoords left, HexCoords right)
    {
        return left.Q != right.Q || left.R != right.R;
    }

    public override string ToString() => $"{q}:{r}";
    /*
    public byte[] ToBytes()
    {
        byte[] bytes = new byte[8];
        int _q;
        int _r;
        // Q
        for (int i = 0; i < 4; i++)
        {
            _q = q >> (i * 8);
            bytes[i] = (byte)_q;
        }
        // R
        for (int i = 4; i < 8; i++)
        {
            _r = r >> (i * 8);
            bytes[i] = (byte)_r;
        }

        return bytes;
    }
    */
}
