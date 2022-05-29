using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LevelEditorNameSpace
{
    public class LevelEditor : LevelLoader
    {


        public GameObject brushCellPrefab;


        public Camera Cam;
        public Brush brush;

        public static LevelEditor Instance;

        protected override void Awake()
        {
            base.Awake();

            if (Instance == null) Instance = this;

            Cam = Camera.main;

            LoadBrushes();
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

            byte[] bytes = new byte[9 * tiles.Count];

            int offset = 0;
            foreach (var tile in tiles)
            {
                byte[] qrCoords = tile.Key.ToBytes();
                for (int i = 0; i < 8; i++)
                {
                    bytes[offset + i] = qrCoords[i];
                }
                bytes[offset + 8] = tile.Value.Type;

                offset += 9; // 4 + 4 bytes are the coords and 1 byte is the type
            }
            if (!Directory.Exists($"{Application.dataPath}/levels")) Directory.CreateDirectory($"{Application.dataPath}/levels");
            File.WriteAllBytes($"{Application.dataPath}/levels/{levelName}.td", Extensions.Compress(bytes));

            Debug.Log("Saved level: " + levelName);
        }


        private void Update()
        {

            brush.Show(HexCoords.CartesianToHex(Cam.ScreenToWorldPoint(Input.mousePosition)));
        }



        public void HandleLeftMouseButton()
        {

            if (LE_InputManager.MouseOverUI) return;


            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out RaycastHit hitinfo);

            HexCoords center = HexCoords.CartesianToHex(hitinfo.point.x, hitinfo.point.z);

            if (brush.IsEreaser)
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

            foreach (HexCoords direction in brush.GetCells())
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

            foreach (HexCoords direction in brush.GetCells())
            {
                HexCoords coord = direction + center;

                Tile current = Instantiate(tilePrefab, HexCoords.HexToCartesian(coord), Quaternion.identity);

                current.Setup(coord, brush.Type);

                current.transform.SetParent(transform, true);

                if (tiles.ContainsKey(coord))
                {
                    if (tiles[coord].Type == current.Type)
                    {
                        Destroy(current.gameObject);
                        continue;
                    }
                    else
                    {
                        RemoveTileAt(coord);
                    }
                }
                current.GetComponent<MeshRenderer>().material.color = brush.Type == 0 ? Color.green : Color.blue;
                tiles.Add(coord, current);
            }

        }

        public void RemoveTileAt(HexCoords coords)
        {
            Destroy(tiles[coords].gameObject);
            tiles.Remove(coords);
        }


        public void SaveBrush()
        {
            byte[] bytes = new byte[8 * tiles.Count];

            int offset = 0;
            foreach (var tile in tiles)
            {
                byte[] qrCoords = tile.Key.ToBytes();
                for (int i = 0; i < 8; i++)
                {
                    bytes[offset + i] = qrCoords[i];
                }

                offset += 8;
            }

            if (Directory.Exists($"{Application.dataPath}/editor/") == false)
            {
                Directory.CreateDirectory($"{Application.dataPath}/editor/");
            }

            int fCount = Directory.GetFiles($"{Application.dataPath}/editor/", "brush_*.tdb", SearchOption.TopDirectoryOnly).Length;
            File.WriteAllBytes($"{Application.dataPath}/editor/brush_{fCount}.tdb", Extensions.Compress(bytes));

            Debug.Log("Saved Brush");
            LoadBrushes(brush.Type);
        }

        public void LoadBrushes(byte type = 0)
        {

            string[] presetPaths = Directory.GetFiles($"{Application.dataPath}/editor/", "brush_*.tdb", SearchOption.AllDirectories);

            brush?.Clear();
            brush = new Brush(presetPaths, type, brush == null ? false : brush.IsEreaser); //Mindjárt elhányom magam

            Debug.Log("Loaded Brushes");
        }

    }
}
