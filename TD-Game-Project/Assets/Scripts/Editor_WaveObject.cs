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
    TMP_InputField enemyCountText = null;
    [SerializeField]
    TMP_InputField enemySpawnAtText = null;
    [SerializeField]
    Button deleteButton = null;

    public WaveObject GetData()
    {
        Debug.Log(byte.Parse(enemyCountText.text) + " is the parsed value");
        return new WaveObject(Util.EnemyId[enemyText.text], byte.Parse(enemyCountText.text), byte.Parse(enemySpawnAtText.text), WaveEditor.SelectedSpawner);
    }

    private void Start()
    {
        deleteButton.onClick.AddListener(delegate { WaveEditor.Singleton.DeleteWaveObject(transform.GetSiblingIndex());});
    }

    public void Setup(string _enemyName)
    {
        enemyText.text = _enemyName;
        enemyImage.sprite = LE_UIManager.Instance.Enemies.FirstOrDefault(x => x.name == _enemyName).icon;
    }

    public void Setup(string _enemyName,byte _enemyCount, byte _spawnAt)
    {
        Setup(_enemyName);
        enemyCountText.text = _enemyCount.ToString();
        enemySpawnAtText.text = _spawnAt.ToString();
    }

}


