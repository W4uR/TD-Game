using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Editor_WaveObject : MonoBehaviour
{
    [SerializeField]
    Image enemyImage = null;
    [SerializeField]
    TMP_Text enemyText = null;
    [SerializeField]
    TMP_Text enemyCountText = null;
    [SerializeField]
    TMP_Text enemySpawnAtText = null;
    [SerializeField]
    Button deleteButton = null;

    private void Start()
    {
        deleteButton.onClick.AddListener(delegate { WaveEditor.Singleton.DeleteWaveObject(transform.GetSiblingIndex()); Destroy(gameObject); });
       
    }

    public void Setup(string _enemyName)
    {
        enemyText.text = _enemyName;
        enemyImage.sprite = LE_UIManager.Instance.Enemies.FirstOrDefault(x => x.name == _enemyName).icon;
    }

}


