using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoords
{
    int q, r;


    static readonly private float width = 9.6f;
    static readonly private float outerRadius = width*0.5f;
    static readonly private float sqrt_3 = Mathf.Sqrt(3f);
    static readonly private float height = sqrt_3 * outerRadius;


    static readonly private Vector3Int[] cube_direction_vectors =
    {
        new Vector3Int( 1, 0, -1 ), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1),
        new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1),
    };

    public static readonly Dictionary<HexCoords, Vector4> neighbour_verticies = new Dictionary<HexCoords, Vector4>()
    {
        {new HexCoords(1, 0), new Vector4(width/4f,height/2f,width/2f,0f)},
        {new HexCoords(1, -1), new Vector4(width/2f,0f,width/4f,-height/2f)},
        {new HexCoords(0, -1), new Vector4(width/4f,-height/2f,-width/4f,-height/2f)},
        {new HexCoords(-1, 0), new Vector4(-width/4f,-height/2f,-width/2f,0f)},
        {new HexCoords(-1, 1), new Vector4(-width/2f,0f,-width/4f,height/2f)},
        {new HexCoords(0, 1), new Vector4(-width/4f,height/2f,width/4f,height/2f)}
    };

    public static List<HexCoords> Neighbors(HexCoords hc)
    {
        List<HexCoords> neighbors = new List<HexCoords>();

        foreach (Vector3Int direction in cube_direction_vectors)
        {
            neighbors.Add(new HexCoords(direction));
        }

        return neighbors;
    }




    public HexCoords(int q, int r)
    {
        this.q = q;
        this.r = r;
    }
    
    public List<HexCoords> Neighbours()
    {
        List<HexCoords> neighbours = new List<HexCoords>();

        foreach (var direction in cube_direction_vectors)
        {
            neighbours.Add( this + new HexCoords(direction));
        }

        return neighbours;
    }

    public static bool IsNeighbour(HexCoords a, HexCoords b)
    {
        HexCoords vec = a - b;
        foreach (var dir in cube_direction_vectors)
        {
            if(dir == vec) return true;
        }
        return false;
    }
    
    public HexCoords(Vector3Int cubeCoords)
    {
        q = cubeCoords.x;
        r = cubeCoords.y;
    }

    
    public HexCoords(byte[] bytes)
    {
        q = BitConverter.ToInt32(bytes);
        r = BitConverter.ToInt32(bytes,4);
    }
    
    public static Vector3 HexToCartesian(HexCoords coords)
    {
        float x = (3f *.5f * coords.q) * outerRadius;
        float y = (sqrt_3 * .5f * coords.q + sqrt_3 * coords.r) * outerRadius;
        return new Vector3(x , 0, y);
    }


    public static HexCoords CartesianToHex(Vector3 pos)
    {
        return CartesianToHex(pos.x, pos.z);
    }

    public static HexCoords CartesianToHex(float x, float y)
    {
        //Do magic here
        float newQ = (2f / 3f) * x / outerRadius;
        float newR = ((-1f / 3f) * x + (sqrt_3 / 3f) * y) / outerRadius;

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


    public static bool operator ==(Vector3Int left, HexCoords right) => left.x == right.q && left.y == right.r;
    public static bool operator !=(Vector3Int left, HexCoords right) => left.x != right.q || left.y != right.r;

    public static bool operator ==(HexCoords left, HexCoords right)
    {
        return left.q == right.q && left.r == right.r;
    }
    public static bool operator !=(HexCoords left, HexCoords right)
    {
        return left.q != right.q || left.r != right.r;
    }

    public static HexCoords operator + (HexCoords left,HexCoords right)
    {
        return new HexCoords(left.q + right.q, left.r + right.r);
    }
    public static HexCoords operator -(HexCoords left, HexCoords right)
    {
        return new HexCoords(left.q - right.q, left.r - right.r);
    }

    public override string ToString() => $"{q} : {r}";
    
    public byte[] ToBytes()
    {
        return Extensions.Concat(BitConverter.GetBytes(q), BitConverter.GetBytes(r));
    }
    
}
