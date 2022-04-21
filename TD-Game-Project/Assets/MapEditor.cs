using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{

    public GameObject tilePrefab;

    private Camera cam;

    private List<Tile> tiles;

    void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        tiles = new List<Tile>();
        for (int i = -10; i < 10; i++)
        {
            for (int j = -6; j < 6; j++)
            {
                GameObject go = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
                Tile current = go.GetComponent<Tile>();
                current.SetHex(i,j);

                tiles.Add(current);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out RaycastHit hitinfo);

            CreateTileAt(hitinfo.point);

        }
    }



    void CreateTileAt(Vector3 pos)
    {


        GameObject go = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);

        go.GetComponent<Tile>().Setup(pos.x, pos.z);

    }


}
