using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace LevelEditorNameSpace {

    public class BrushSelector : MonoBehaviour
    {
        [SerializeField]
        GameObject Button_Brush_Prefab = null;
        [SerializeField]
        Slider slider_Brush = null;

        public Brush SelectedBrush { get; private set; }
        public TileType SelectedType { get; private set; }

        public void LoadBrushes()
        {
            //--Clearing
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i));
            }
            SelectedBrush?.Clear();
            //-----

            string[] presetPaths = Directory.GetFiles($"{Application.dataPath}/editor/", "brush_*.tdb", SearchOption.AllDirectories);
            SelectedBrush = new Brush(presetPaths, SelectedType, SelectedBrush == null ? false : SelectedBrush.IsEreaser); //Mindjárt elhányom magam

            Instantiate(Button_Brush_Prefab, transform);
            for (int i = 0; i < presetPaths.Length; i++)
            {
                GameObject brush_button = Instantiate(Button_Brush_Prefab, transform);

                var index = i;
                brush_button.GetComponent<Button>().onClick.AddListener(delegate { SelectBrush(index); });
            }

            Debug.Log("Loaded Brushes");
        }

        private void Start()
        {
            slider_Brush.onValueChanged.AddListener(OnSliderValueChanged);
            LoadBrushes();
        }

        public void OnSliderValueChanged(float val)
        {

        }

        void SelectBrush(int index)
        {

        }
    }

}
