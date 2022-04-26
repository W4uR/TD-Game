using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class NetworkRoomPlayer : NetworkBehaviour
{

    [Header("UI")]
    [SerializeField] private TMP_Text playerNameText = null;
    [SerializeField] private TMP_Text characterText = null;
    [SerializeField] private TMP_Text readyText = null;


    private NetworkRoomManager room;
    private NetworkRoomManager Room
    {
        get
        {
            if (room != null) return room;
            return room = NetworkManager.singleton as NetworkRoomManager;
        }
    }



    [SyncVar(hook = nameof(OnReadyStatusChanged))]
    public bool IsReady = false;
    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    public string PlayerName;
    [SyncVar(hook = nameof(OnCharacterChanged))]
    public int SelectedCharacterIndex = 0;


    public override void OnStartAuthority()
    {
        CmdSetPlayerNameText(MenuUIManager.PlayerName);

        RoomUIManager.singleton.StartGameButton.onClick.AddListener(() =>
        {
            CmdStartGame();
        });
        RoomUIManager.singleton.ReadyGameButton.onClick.AddListener(() =>
        {
            CmdToggleReady();
        });
    }

    #region Commands
    [Command]
    private void CmdSetPlayerNameText(string playerName)
    {
        PlayerName = playerName;
    }
    [Command]
    private void CmdSetCharaterIndex(int newIndex)
    {
        SelectedCharacterIndex = newIndex;
    }

    [Command]
    private void CmdToggleReady()
    {
        IsReady = !IsReady;
        Room.NotifyLeaderOfReadyState();
    }

    [Command]
    private void CmdStartGame(NetworkConnectionToClient conn = null)
    {
        if (Room.Leader != conn) return;

        //Start Game
    }

    #endregion

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);
    }
    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
    }


    private void OnReadyStatusChanged(bool oldV, bool _isReady)
    {
        readyText.text = _isReady ? "<color=green>Ready</color>": "<color=red>Not Ready</color>";
        if(hasAuthority)
        RoomUIManager.singleton.SetReadyButtonText(_isReady);
    }
    private void OnPlayerNameChanged(string oldV, string newV)
    {
        playerNameText.text = newV;
    }

    private void OnCharacterChanged(int oldV, int newV)
    {
        characterText.text = RoomUIManager.singleton.charaters[newV].name;
    }

    internal void HandleReadyToStart(bool readyToStart)
    {
        RoomUIManager.singleton.StartGameButton.interactable = readyToStart;
    }



    public void SetAsLeader()
    {
        RoomUIManager.singleton.ShowStartButton();
    }

}
