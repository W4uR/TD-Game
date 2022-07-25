using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaveEditor : MonoBehaviour
{
    public static byte SelectedSpawner;
    private byte selectedWave = 0; //starts at 0

    private static List<Wave> waves;

    [Header("Waves")]
    [SerializeField]
    Transform wave_Holder = null;
    [SerializeField]
    GameObject Button_Wave_Prefab = null;
    [SerializeField]
    Button Button_Add_Wave = null;


    [Header("Wave Objects")]
    [SerializeField]
    Transform waveObject_Holder = null;
    [SerializeField]
    GameObject Card_WaveObject_Prefab = null;
    [SerializeField]
    Button Button_Add_WaveObject = null;

    

    public static WaveEditor Singleton = null;

    public byte SelectedWave { get => Singleton.selectedWave; 
        set {
            //Save current waveobjects
            Save();

            selectedWave = value;

            for (int i = 0; i < wave_Holder.childCount-1; i++)
            {
                wave_Holder.GetChild(i).GetComponent<WaveButton>().SetAsSelected(i == value);
            }
            //Everytime we update the selected wave, we want to load in the proper waveobject datas
            LoadSelectedWave();
        } 
    }

    public static List<Wave> Waves { get => waves;
        set {
            waves = value;
            Singleton.LoadWaves();
        }
    }

    public WaveEditor()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        
        
    }
    private void Awake()
    {
        if (Waves == null || Waves.Count == 0)
        {
            Waves = new List<Wave>();
            Waves.Add(new Wave());
        }
    }

    private void LoadSelectedWave()
    {
        //Get the waveobject datas from the seleceted wave
        List<WaveObject> waveObjects = Waves[selectedWave].GetWaveObjects(SelectedSpawner);
        
        //Clear the waveobject holder (UI)
        ClearWaveObjectHolder();

        foreach (var waveObject in waveObjects)
        {
            //Instantiate waveobject with the proper datas
            var card = Instantiate(Card_WaveObject_Prefab, waveObject_Holder);
            card.GetComponent<Editor_WaveObject>().Setup(Util.EnemyName[waveObject.EnemyId],waveObject.NumberOfEnemies,waveObject.SpawnTime);
        }
        //Only show the waveobject adding button if there are less than 11 waveobject cards
        Button_Add_WaveObject.gameObject.SetActive(waveObject_Holder.childCount <= 12);
        //Reposition the adding button
        Button_Add_WaveObject.transform.SetAsLastSibling();

        Debug.Log("Selected Wave is "+ ( 1 + SelectedWave));
    }

    private void ClearWaveObjectHolder()
    {
        //Destroy children (waveobject cards) except for the add button
        for (int i = waveObject_Holder.childCount-2; i >= 0; i--)
        {
            Util.MurderChildNoWitnesses(waveObject_Holder.GetChild(i).gameObject);
        }
    }


    //Referenced on Button_Add_Wave
    internal void OpenWindow()
    {
        gameObject.SetActive(true);
        LoadSelectedWave();
    }
    public void CloseWindow()
    {
        Save();
        gameObject.SetActive(false);
    }

    private void LoadWaves()
    {
        Util.MurderChildNoWitnesses(wave_Holder.GetChild(0).gameObject);
        foreach (var wave in Waves)
        {
            Instantiate(Button_Wave_Prefab, wave_Holder);
        }
        Button_Add_Wave.transform.SetAsLastSibling();
        for (int i = 0; i < wave_Holder.childCount - 1; i++)
        {
            wave_Holder.GetChild(i).GetComponent<WaveButton>().UpdateObject();
        }
    }

    public void AddWave()
    {
        //Create wave button
        Instantiate(Button_Wave_Prefab,wave_Holder);
        //Add empty wave to waves list
        Waves.Add(new Wave());
        //Reposition the wave adding button
        Button_Add_Wave.transform.SetAsLastSibling();

        for (int i = 0; i < wave_Holder.childCount - 1; i++)
        {
            wave_Holder.GetChild(i).GetComponent<WaveButton>().UpdateObject();
        }
    }
    public void DeleteWave(int waveIndex)
    {
        
        //Get rid of the wave button
        Util.MurderChildNoWitnesses(wave_Holder.GetChild(waveIndex).gameObject);
        //Delete wave from list
        Waves.RemoveAt(waveIndex);
        //Simply reseteing the selected wave
        SelectedWave = 0;

        for (int i = 0; i < wave_Holder.childCount-1; i++)
        {
            wave_Holder.GetChild(i).GetComponent<WaveButton>().UpdateObject();
        }
    }

    public void DeleteWaveObject(int waveObjectIndex)
    {
        //Get rid of the waveobject card from the UI
        Util.MurderChildNoWitnesses(waveObject_Holder.GetChild(waveObjectIndex).gameObject);
        //Delete the logical representation of the waveobject
        Waves[SelectedWave].DeleteWaveObject(waveObjectIndex);

        //Show the waveobject adding button when there are less than 12 waves
        if (waveObject_Holder.childCount <= 12)
            Button_Add_WaveObject.gameObject.SetActive(true);
    }


    public void AddWaveObject(string enemyName)
    {
        //Create a new waveobject card
        var newCard = Instantiate(Card_WaveObject_Prefab, waveObject_Holder);
        
        //Set default values for the card
        newCard.GetComponent<Editor_WaveObject>().Setup(enemyName);
        //Set default values for the logical representation
        Waves[SelectedWave].AddWaveObject(new WaveObject(Util.EnemyId[enemyName], 1, 0, SelectedSpawner));

        //Reposition the waveobject adding button
        Button_Add_WaveObject.transform.SetAsLastSibling();

        //Hide the waveobject adding button when there are more than 11 waves
        if (waveObject_Holder.childCount > 12)
            Button_Add_WaveObject.gameObject.SetActive(false);
    }
    public void Save()
    {
        for (int i = 0; i < waveObject_Holder.childCount-1; i++)
        {
            Waves[selectedWave].SetWaveObject(i, waveObject_Holder.GetChild(i).GetComponent<Editor_WaveObject>().GetData());
        }
    }
}
