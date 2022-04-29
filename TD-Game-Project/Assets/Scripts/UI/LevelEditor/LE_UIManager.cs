using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LevelEditorNameSpace
{
    public class LE_UIManager : MonoBehaviour
    {
        [SerializeField] private LevelEditor levelEditor = null;
        
        public static LE_UIManager Instance;

        [Header("UI")]
        [SerializeField] TMP_Text Ereaser_text;

        [Header("Level Saving/Loading")]
        [SerializeField] TMP_InputField levelName_InputField = null;
        [SerializeField] TMP_Dropdown levelName_DropDown = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        //Put UI methods here
        public void SaveLevel()
        {

        }

        public void LoadLevel()
        {
            string levelName = levelName_DropDown.options[levelName_DropDown.value].text;

        }


        public void SetIsEreaser(bool isEreser)
        {
            Ereaser_text.text = isEreser ? "Ereaser" : "Brush";
        }
    }
}
