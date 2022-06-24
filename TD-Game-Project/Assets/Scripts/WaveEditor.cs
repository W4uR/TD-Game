using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveEditor : MonoBehaviour
{
    public static int SelectedSpawner;
    public static int SelectedWave;

    [SerializeField]
    Transform WaveSelector = null;
    [SerializeField]
    GameObject Button_Wave_Prefab = null;
    [SerializeField]
    Button Button_Add_Wave = null;
    
    public static event Action WaveListUpdated;

    private void Start()
    {
        Button_Add_Wave.onClick.AddListener(AddWave);
    }

    public void OpenWindow()
    {

    }

    public void CloseWindow()
    {

    }

    public void AddWave()
    {
        Instantiate(Button_Wave_Prefab,WaveSelector);
        LevelLoader.AddWave();
        Button_Add_Wave.transform.SetAsLastSibling();
        WaveListUpdated.Invoke();
    }
    public static void DeleteWave(int wave, GameObject callerButton)
    {
        
        callerButton.transform.SetParent(null);
        Destroy(callerButton);

        LevelLoader.DeleteWave(wave);
        WaveListUpdated.Invoke();
    }

}
