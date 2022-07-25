using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System;

namespace LevelEditorNameSpace
{
    public class EnemySelectorPanel : MonoBehaviour
    {
        [SerializeField]
        TMP_Dropdown enemyDropdown = null;

        private void Start()
        {
            List<string> enemies = LE_UIManager.Instance.Enemies.Select(x=>x.name).ToList();
            enemyDropdown.AddOptions(enemies);

        }

        //Referenced on Button_Add_Enemy
        public void OnClickAdd()
        {
            WaveEditor.Singleton.AddWaveObject(enemyDropdown.options[enemyDropdown.value].text);
            gameObject.SetActive(false);
        }
    }

}
