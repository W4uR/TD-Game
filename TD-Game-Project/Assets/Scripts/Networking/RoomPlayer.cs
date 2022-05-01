using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public class RoomPlayer : NetworkBehaviour
{

    [Header("Charaters")]
    [SerializeField] public Character[] Charaters = new Character[0];

    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private Image[] playerCharaterImages = new Image[0];
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[0];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[0];
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private TMP_Dropdown levelSelectDropdown = null;

    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;
    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleCharaterChanged))]
    public int CharaterIndex = 0;

    public bool IsLeader { set { startGameButton.gameObject.SetActive(value); levelSelectDropdown.gameObject.SetActive(true); FillLevelDropdown() ; } }
    private NetworkManagerTDGame room;
    private NetworkManagerTDGame Room
    {
        get { if (room != null) return room; return room = NetworkManager.singleton as NetworkManagerTDGame; }
    }

    public Character GetSelectedCharater => Charaters[CharaterIndex];


    public void SelectLevel()
    {
        string levelName = levelSelectDropdown.options[levelSelectDropdown.value] + ".td";
        //Room.LevelData = Extensions.Decompress(File.ReadAllBytes($"{Application.dataPath}/levels/{levelName}.td"));
    }

    

    private void FillLevelDropdown()
    {
        if (!Directory.Exists($"{Application.dataPath}/levels")) Directory.CreateDirectory($"{Application.dataPath}/levels");

        string[] levels = Directory.GetFiles($"{Application.dataPath}/levels/", "*.td", SearchOption.TopDirectoryOnly).Select(x => x.Split('/').Last().Split('.').First()).ToArray();

        levelSelectDropdown.options.Clear();

        foreach (var level in levels)
        {
            levelSelectDropdown.options.Add(new TMP_Dropdown.OptionData(level));
        }
        levelSelectDropdown.RefreshShownValue();
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(MenuUIManager.PlayerName);
        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }


    [Command]
    public void CmdSetDisplayName(string playerName)
    {
        DisplayName = playerName;
    }
    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyLeaderOfReadyState();
    }

    [Command]
    public void CmdSelectCharater(int index)
    {
        CharaterIndex = index;

    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.Leader != this) return;
        // Start Game
        Debug.Log("Game started");
        Room.StartGame();
    }

    public void LeaveRoom()
    {
        Room.StopHost();
    }

    private void HandleDisplayNameChanged(string _, string name) => UpdateDisplay();

    private void HandleReadyStatusChanged(bool _, bool isReady) => UpdateDisplay();

    private void HandleCharaterChanged(int _, int charIndex) => UpdateDisplay();

    internal void HandleReadyToStart(bool isReady2Start)
    {
        startGameButton.interactable = isReady2Start;
    }

    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for Player...";
            playerReadyTexts[i].text = string.Empty;
            playerCharaterImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";

            playerCharaterImages[i].sprite = Charaters[Room.RoomPlayers[i].CharaterIndex].icon;

            playerCharaterImages[i].gameObject.SetActive(true);
        }



    }

}
