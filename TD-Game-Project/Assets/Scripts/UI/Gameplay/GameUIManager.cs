using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel = null;
    private NetworkManagerTDGame room;
    private NetworkManagerTDGame Room
    {
        get { if (room != null) return room; return room = NetworkManager.singleton as NetworkManagerTDGame; }
    }
    private void OnEnable()
    {
        LevelLoader.OnLevelLoaded += OnLevelLoaded;
    }
    private void OnDisable()
    {
        LevelLoader.OnLevelLoaded -= OnLevelLoaded;
    }

    public void OnLevelLoaded()
    {
        Destroy(loadingPanel);
    }

    public void Leave()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            Room.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            Room.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            Room.StopServer();
        }
    }
}
