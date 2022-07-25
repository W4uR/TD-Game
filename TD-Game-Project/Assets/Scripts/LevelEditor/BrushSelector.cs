using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrushSelector : MonoBehaviour
{

    [SerializeField]
    GameObject Button_Brush_Prefab = null;

    [Serializable]
    private struct Preset
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
        previewCells = new List<GameObject>();
        LoadBrushes();
    }


    public void ToggleEreaser()
    {
        IsEreaser = !IsEreaser;
    }

    void SetBrush(int index)
    {
        brushIndex = index;
    }

    public void SetType(TileType type)
    {
        SelectedType = type;
    }



    [SerializeField]

    private GameObject brushCellPrefab;
    private List<GameObject> previewCells;

    internal void ShowPreview(HexCoords center)
    {

        foreach (var previewCell in previewCells)
        {
            GameObject.Destroy(previewCell);
        }
        previewCells.Clear();

        if (LE_InputManager.MouseOverUI) return;

        foreach (HexCoords direction in SelectedBrush.GetCells())
        {
            HexCoords coord = direction + center;

            GameObject current = Instantiate(brushCellPrefab, HexCoords.HexToCartesian(coord) + Vector3.up, Quaternion.identity);
            previewCells.Add(current);
        }

    }
}

