using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaveEditor : MonoBehaviour
{
    public static byte SelectedSpawner;
    private byte selectedWave;

    public static List<Wave> Waves;

    [Header("Waves")]
    [SerializeField]
    Transform WaveSelector = null;
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


    
    public static event Action WaveListUpdated;


    public static WaveEditor Singleton = null;

    public static byte SelectedWave { get => Singleton.selectedWave; 
        set {
            Singleton.selectedWave = value;
            Singleton.LoadSelectedWave();
        } 
    }

    private void LoadSelectedWave()
    {
        List<WaveObject> waveObjects = Waves[selectedWave].GetWaveObjects();
        ClearWaveObjectHolder();

        foreach (var waveObject in waveObjects)
        {
            //Instantiate waveobject with the proper datas
        }
        Button_Add_WaveObject.transform.SetAsLastSibling();
        throw new NotImplementedException();
    }

    private void ClearWaveObjectHolder()
    {
        //Destroy children except for the add button
        throw new NotImplementedException();
    }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        Waves = new List<Wave>();
        Waves.Add(new Wave());
    }

    //Referenced on Button_Add_Wave
    public void AddWave()
    {
        Instantiate(Button_Wave_Prefab,WaveSelector);
        Waves.Add(new Wave());
        Button_Add_Wave.transform.SetAsLastSibling();
        WaveListUpdated.Invoke();
    }
    public void DeleteWave(int waveIndex, GameObject callerButton)
    {
        
        callerButton.transform.SetParent(null);
        Destroy(callerButton);

        Waves.RemoveAt(waveIndex);

        WaveListUpdated.Invoke();
    }

    public void DeleteWaveObject(int waveObjectIndex)
    {
        Waves[SelectedWave].DeleteWaveObject(waveObjectIndex);
    }

    public void AddWaveObject(string enemyName)
    {
        Debug.Log("Instantiate " + enemyName);
        var newCard = Instantiate(Card_WaveObject_Prefab, waveObject_Holder);
        newCard.GetComponent<Editor_WaveObject>().Setup(enemyName);
        Waves[SelectedWave].AddWaveObject(new WaveObject(LE_UIManager.Instance.Enemies.First(x => x.name == enemyName).id, 1, 0, SelectedSpawner));
        Button_Add_WaveObject.transform.SetAsLastSibling();
    }


    public void UpdateWaveObject()
    {

    }
}
