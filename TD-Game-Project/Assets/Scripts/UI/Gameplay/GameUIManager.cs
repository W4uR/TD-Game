using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel = null;

    public static GameUIManager Singleton = null;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
        {
            Debug.LogError("There can be only one LevelLoader");
        }
    }


    public void OnLevelLoaded()
    {
        Destroy(loadingPanel);
    }
}
