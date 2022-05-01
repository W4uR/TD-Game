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
    private Character playerCharater = null;

    [SerializeField] GameObject gameplayPlayerPrefab;

    private GameObject myPlayer;

    [SerializeField]

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
    public void SetCharater(Character _char)
    {
        playerCharater = _char;
    }

    [Command]
    public void CmdSpawnAt(Vector3 pos)
    {
        myPlayer = Instantiate(gameplayPlayerPrefab,pos,Quaternion.identity);
        NetworkServer.Spawn(myPlayer, gameObject);
    }
}
