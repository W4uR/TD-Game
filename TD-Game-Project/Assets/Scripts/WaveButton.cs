using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveButton : MonoBehaviour
{
    [SerializeField]
    Button deleteButton = null;
    [SerializeField]
    TMP_Text waveLabel;

    void Start()
    {
        deleteButton.onClick.AddListener(delegate { WaveEditor.DeleteWave(transform.GetSiblingIndex(), gameObject); });
    }
    private void OnEnable()
    {
        WaveEditor.WaveListUpdated += HandleWaveListUpdated;
    }
    private void OnDestroy()
    {
        WaveEditor.WaveListUpdated -= HandleWaveListUpdated;
    }

    private void HandleWaveListUpdated()
    {

        deleteButton.gameObject.SetActive(LevelLoader.WaveCount > 1);
        waveLabel.text = (transform.GetSiblingIndex()+1).ToString();
    }

    public void OnButtonClicked()
    {
        WaveEditor.SelectedWave = transform.GetSiblingIndex();
        Debug.Log(WaveEditor.SelectedWave);
    }


}
