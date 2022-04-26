using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{

    [SerializeField] private NetworkRoomManager networkManager = null;

    [Header("Panels")]
    [SerializeField] private GameObject roomPanel = null;
    [SerializeField] private GameObject menuPanel = null;
    [Header("UI")]
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button joinButton = null;


    public static string PlayerName { get; private set; }

    private const string PLAYERNAME_KEY = "PlayerName";




    private void Start()
    {
        SetUpInputField();
    }


    private void Awake()
    {
        NetworkRoomManager.OnClientConnected += HandleClientConnected;
        NetworkRoomManager.OnClientDisconnected += HandleClientDisconnected;
    }
    private void OnDestroy()
    {
        NetworkRoomManager.OnClientConnected -= HandleClientConnected;
        NetworkRoomManager.OnClientDisconnected -= HandleClientDisconnected;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;
        menuPanel.SetActive(false);
        roomPanel.SetActive(true);//false
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
        RoomUIManager.singleton.HideStartButton();     
    }

    public void LeaveRoom()
    {
        networkManager.StopHost();
        networkManager.StopClient();
        roomPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void HostRoom()
    {
        networkManager.StartHost();
    }
    public void JoinRoom()
    {
        string ipAddress = ipAddressInputField.text;
        if (String.IsNullOrEmpty(ipAddress)) return;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }


    #region Settings
    private void SetUpInputField()
    {
        string defaultName = PlayerPrefs.HasKey(PLAYERNAME_KEY) ? PlayerPrefs.GetString(PLAYERNAME_KEY) : Util.GenerateRandomName();
        nameInputField.text = defaultName;
    }
    public void SavePlayerName()
    {
        PlayerName = nameInputField.text;
        PlayerPrefs.SetString(PLAYERNAME_KEY, PlayerName);
    }
    public void RandomName()
    {
        nameInputField.text = Util.GenerateRandomName();
    }


    #endregion
}
