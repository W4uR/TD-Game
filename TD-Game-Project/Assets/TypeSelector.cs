using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditorNameSpace
{
    public class TypeSelector : MonoBehaviour
    {
        [SerializeField]
        GameObject Button_Type_Prefab = null;

        [SerializeField]
        BrushSelector brushSelector = null;

        [Serializable]
        public struct Types
        {
            public TileType number;
            public Sprite image;
        }
        [SerializeField] Types[] types;

        private void Start()
        {
            foreach (var type in types)
            {
                GameObject button = Instantiate(Button_Type_Prefab, transform);
                button.GetComponent<Image>().sprite = type.image;
                button.GetComponent<Button>().onClick.AddListener(delegate {OnTypeButtonClicked(type.number); });
            }
        }

        void OnTypeButtonClicked(TileType selectedType)
        {
            Debug.Log(selectedType + " type is selected");
            brushSelector.SetType(selectedType);
        }
    }
}