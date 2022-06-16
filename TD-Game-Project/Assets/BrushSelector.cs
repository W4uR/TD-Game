using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace LevelEditorNameSpace {

    public class BrushSelector : MonoBehaviour
    {

        [SerializeField]
        GameObject Button_Brush_Prefab = null;

        [Serializable]
        public struct Preset
        {
            public List<HexCoords> coords;
            public Sprite image;
        }
        [SerializeField] Preset[] presets;



        public TileType SelectedType { get; private set; }
        public bool IsEreaser { get; private set; }
        public BrushPreset SelectedBrush => brushes[brushIndex];


        private List<BrushPreset> brushes;
        private int brushIndex = 0;
        
        public void LoadBrushes()
        {
            int index = 0;
            foreach (var preset in presets)
            {
                GameObject button = Instantiate(Button_Brush_Prefab, transform);
                button.GetComponent<Image>().sprite = preset.image;
                int currentIndex = index;
                button.GetComponent<Button>().onClick.AddListener(delegate { SetBrush(currentIndex); });
                brushes.Add(new BrushPreset(preset.coords));
                index++;
            }
        }
        
        private void Start()
        {
            brushes = new List<BrushPreset>();

            LoadBrushes();
        }


        public void ToggleEreaser()
        {
            IsEreaser = !IsEreaser;
            Debug.Log(IsEreaser);
        }

        void SetBrush(int index)
        {
            Debug.Log(index + ". brush preset was selected");
            brushIndex = index;
        }

        public void SetType(TileType type)
        {
            SelectedType = type;
        }


    }

}
