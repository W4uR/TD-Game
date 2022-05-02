using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class GamePlayer : NetworkBehaviour
{

    [SyncVar]
    private string displayName = string.Empty;
    [SyncVar(hook =nameof(ApplyCharacter))]
    private int characterIndex = -1;

    public Character GetSelectedCharacter => Room.Characters[characterIndex];
    [SerializeField] MeshFilter myModel = null;

    private NetworkManagerTDGame room;
    private NetworkManagerTDGame Room
    {
        get { if (room != null) return room; return room = NetworkManager.singleton as NetworkManagerTDGame; }
    }
    public override void OnStartServer() => NetworkManagerTDGame.OnServerReadied += LoadLevelForConnection;

    #region Network

    [ServerCallback]
    private void OnDestroy() => NetworkManagerTDGame.OnServerReadied -= LoadLevelForConnection;


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
    [Server]
    public void SetCharater(int _characterIndex)
    {
        characterIndex = _characterIndex;
    }
    public void ApplyCharacter(int _,int characterIndex)
    {
        myModel.mesh = GetSelectedCharacter.model;
    }


    [Server]
    private void LoadLevelForConnection(NetworkConnectionToClient conn)
    {
        LoadLevel(conn,Room.LevelData);
        
    }
    [TargetRpc]
    private void LoadLevel(NetworkConnection target, byte[] levelData)
    {
        LevelLoader.Singleton.LoadLevel(levelData);
        GameUIManager.Singleton.OnLevelLoaded();
        CmdSpawnAtRandom();
    }
    [Command]
    private void CmdSpawnAtRandom()
    {
        transform.position = LevelLoader.Singleton.GetRandomSpawnPoint().Value;
    }



    #endregion
}
