using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

namespace LevelEditorNameSpace
{
    public class LevelEditor : LevelLoader
    {

        [SerializeField]

        public GameObject brushCellPrefab;

        public Camera Cam;

        [SerializeField]
        BrushSelector brushSelector;


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

            byte[] bytes = new byte[4+(Tile.Size * tiles.Count)+(waves.Sum(x=>x.Size)+waves.Count)];

            bytes[0] = 0;//Version
            bytes[1] = BitConverter.GetBytes(tiles.Count)[0];//lower byte
            bytes[2] = BitConverter.GetBytes(tiles.Count)[1];//higher byte
            bytes[3] = (byte)waves.Count;

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
            foreach (var wave in waves)
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

        /*
        private void Update()
        {
            brush.Show(HexCoords.CartesianToHex(Cam.ScreenToWorldPoint(Input.mousePosition)));
        }
        */


        public void HandleLeftMouseButton()
        {

            if (LE_InputManager.MouseOverUI) return;


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

            foreach (HexCoords direction in brushSelector.SelectedBrush.GetCells())
            {
                HexCoords coord = center + direction;

                Tile current = CreateTile(coord);

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
                current.GetComponent<MeshRenderer>().material.color = brushSelector.SelectedType == 0 ? Color.green : Color.blue;
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
            byte[] bytes = new byte[HexCoords.Size * tiles.Count];

            int offset = 0;
            foreach (var tile in tiles)
            {
                byte[] qrCoords = tile.Key.ToBytes();
                for (int i = 0; i < 8; i++)
                {
                    bytes[offset + i] = qrCoords[i];
                }

                offset += HexCoords.Size;
            }

            if (Directory.Exists($"{Application.dataPath}/editor/") == false)
            {
                Directory.CreateDirectory($"{Application.dataPath}/editor/");
            }

            int fCount = Directory.GetFiles($"{Application.dataPath}/editor/", "brush_*.tdb", SearchOption.TopDirectoryOnly).Length;
            File.WriteAllBytes($"{Application.dataPath}/editor/brush_{fCount}.tdb", Extensions.Compress(bytes));

            Debug.Log("Saved Brush");
            brushSelector.LoadBrushes();
        }


        private Tile CreateTile(HexCoords coord)
        {
            Tile current = Instantiate(tilePrefab, HexCoords.HexToCartesian(coord), Quaternion.identity);

            current.Setup(coord, brushSelector.SelectedType);

            current.transform.SetParent(transform, true);
            return current;
        }


    }

}
