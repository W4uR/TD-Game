using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoords
{
    int q, r;

    

    static readonly private float width = 2f;
    static readonly private float height = Mathf.Sqrt(3f);

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

    public static Vector3 HexToCartesian(HexCoords coords)
    {
        float x = 3f *.5f * coords.Q;
        float y = Mathf.Sqrt(3f) * .5f * coords.Q - Mathf.Sqrt(3f) * coords.R;
        return new Vector3(x * .5f, 0, -y*.5f);
    }

    public static HexCoords CartesianToHex(float x, float y)
    {
        //Do magic here
        float newQ = (2 * x / 3);
        float newR = ((-x / 3) + (y * Mathf.Sqrt(3) / 3));


        return new HexCoords(RoundCoords(newQ,newR));
    }

    static Vector3Int RoundCoords(float _q, float _r)
    {
        var _s = (-_q - _r);

        int _Q = (int)Mathf.Round(_q);
        int _R = (int)Mathf.Round(_r);
        int _S = (int)Mathf.Round(-_s);

        var q_diff = Mathf.Abs(_Q - _q);
        var r_diff = Mathf.Abs(_R - _r);
        var s_diff = Mathf.Abs(_S - _s);

        if (q_diff > r_diff && q_diff > s_diff)
            _Q = -_R - _S;
        else if (r_diff > s_diff)
            _R = -_Q - _S;
        else
            _S = -_Q - _R;

        return new Vector3Int(_R, _Q, _S);
    }

}
