using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{

    private NetworkManagerTDGame networkManagerTD;
    private NetworkManagerTDGame NetworkManagerTD
    {
        get { if (networkManagerTD != null) return networkManagerTD; return networkManagerTD = NetworkManager.singleton as NetworkManagerTDGame; }
    }



    [Header("UI")]
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button joinButton = null;

    [HideInInspector]
    public static string PlayerName { get; private set; }

    private const string PLAYERNAME_KEY = "PlayerName";

    private void OnEnable()
    {
        NetworkManagerTDGame.OnClientConnected += HandleClientConnect;
        NetworkManagerTDGame.OnClientDisconnected += HandleClientDisconnect;
    }
    private void OnDisable()
    {
        NetworkManagerTDGame.OnClientConnected -= HandleClientConnect;
        NetworkManagerTDGame.OnClientDisconnected -= HandleClientDisconnect;
    }

    private void HandleClientConnect()
    {
        joinButton.interactable = true;
    }
    private void HandleClientDisconnect()
    {
        joinButton.interactable = true;
    }


    private void Start()
    {
        SetUpInputField();
    }

    #region UI Methods
    public void HostRoom()
    {
        NetworkManagerTD.StartHost();
    }
    public void JoinRoom()
    {
        string ipAddress = ipAddressInputField.text;
        if (String.IsNullOrEmpty(ipAddress)) return;

        NetworkManagerTD.networkAddress = ipAddress;
        NetworkManagerTD.StartClient();

        joinButton.interactable = false;
    }

    public void LoadEditor()
    {
        SceneManager.LoadScene("Editor");
    }

    public void RandomName()
    {
        nameInputField.text = Util.GenerateRandomName();
    }

    public void SavePlayerName()
    {
        PlayerName = nameInputField.text;
        PlayerPrefs.SetString(PLAYERNAME_KEY, PlayerName);
    }
    #endregion

    #region Settings
    private void SetUpInputField()
    {
        string defaultName = PlayerPrefs.HasKey(PLAYERNAME_KEY) ? PlayerPrefs.GetString(PLAYERNAME_KEY) : Util.GenerateRandomName();
        nameInputField.text = defaultName;
    }
 
    #endregion
}
