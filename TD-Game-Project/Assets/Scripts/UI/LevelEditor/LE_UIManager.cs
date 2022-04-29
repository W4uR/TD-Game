using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

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
            FillDropDown();
        }

        //Put UI methods here
        public void LoadMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void SaveLevel()
        {
            if (levelName_InputField.text == string.Empty)
            {
                Debug.LogError("Must give a name to the level before saving");
                return;
            }
            if (levelName_InputField.text.Contains('.'))
            {
                Debug.LogError("Level name must not contain '.' charater");
                return;
            }
            levelEditor.SaveLevel(levelName_InputField.text);
            FillDropDown();
        }

        public void LoadLevel()
        {
            if (levelName_DropDown.options.Count == 0) return;

            string levelName = levelName_DropDown.options[levelName_DropDown.value].text;
            levelEditor.LoadLevel(levelName);
        }

        public void DeleteLevel()
        {
            if (levelName_DropDown.options.Count == 0) return;

            string levelName = levelName_DropDown.options[levelName_DropDown.value].text;
            if (!File.Exists($"{Application.dataPath}/levels/{levelName}.td")) return;
            File.Delete($"{Application.dataPath}/levels/{levelName}.td");
#if UNITY_EDITOR
            File.Delete($"{Application.dataPath}/levels/{levelName}.td.meta");
#endif
            FillDropDown();
        }

        private void FillDropDown()
        {
            if (!Directory.Exists($"{Application.dataPath}/levels")) Directory.CreateDirectory($"{Application.dataPath}/levels");

            string[] levels = Directory.GetFiles($"{Application.dataPath}/levels/", "*.td", SearchOption.TopDirectoryOnly).Select(x => x.Split('/').Last().Split('.').First()).ToArray();

            levelName_DropDown.options.Clear();

            foreach (var level in levels)
            {
                levelName_DropDown.options.Add(new TMP_Dropdown.OptionData(level));
            }
            levelName_DropDown.RefreshShownValue();
        }

        public void SetIsEreaser(bool isEreser)
        {
            Ereaser_text.text = isEreser ? "Ereaser" : "Brush";
        }
    }
}
