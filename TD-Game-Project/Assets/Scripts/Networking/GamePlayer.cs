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
    private int Id = -1;
    [SyncVar]
    private string displayName = string.Empty;
    [SyncVar(hook =nameof(ApplyCharacter))]
    private int characterIndex = -1;

    [SerializeField]
    MeshFilter myMesh;

    public Character GetSelectedCharacter => Room.Characters[characterIndex];

    private NetworkManagerTDGame room;
    private NetworkManagerTDGame Room
    {
        get { if (room != null) return room; return room = NetworkManager.singleton as NetworkManagerTDGame; }
    }


    #region Network
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        CmdRegisterMyself();
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }
    [Command]
    private void CmdRegisterMyself()
    {
        Id = Room.RoomPlayers.Count;
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
    public void ApplyCharacter(int _, int index)
    {
        myMesh.mesh = Room.Characters[index].model;
    }
    #endregion
}
