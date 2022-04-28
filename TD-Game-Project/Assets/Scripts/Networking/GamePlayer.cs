using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GamePlayer : NetworkBehaviour
{

    [SyncVar]
    private string displayName = string.Empty;

    private NetworkManagerTDGame room;
    private NetworkManagerTDGame Room
    {
        get { if (room != null) return room; return room = NetworkManager.singleton as NetworkManagerTDGame; }
    }


    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
        name = displayName;
    }
}
