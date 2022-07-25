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
        deleteButton.onClick.AddListener(delegate { WaveEditor.Singleton.DeleteWave(transform.GetSiblingIndex()); });
        waveLabel.fontStyle = FontStyles.Normal;
    }

    public void SetAsSelected(bool isSelected)
    {
        if (isSelected)
        {
            waveLabel.fontStyle = FontStyles.Underline;
            return;
        }
        waveLabel.fontStyle = FontStyles.Normal;

    }

    public void UpdateObject()
    {
        deleteButton.gameObject.SetActive(WaveEditor.Waves.Count > 1);
        waveLabel.text = (transform.GetSiblingIndex()+1).ToString();      
    }

    //Referenced on the object
    public void OnButtonClicked()
    {
        WaveEditor.Singleton.SelectedWave = (byte)transform.GetSiblingIndex();
    }


}
