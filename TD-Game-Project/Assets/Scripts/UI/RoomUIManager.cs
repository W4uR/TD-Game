using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIManager : MonoBehaviour
{

    [Header("NetworkManager")]
    public NetworkRoomManager Room;

    [Header("UI")]
    public Button ReadyGameButton;
    public Button StartGameButton;

    public Transform RoomPlayerParent;


    [Header("Extra")]
    public Charater[] charaters;


    [HideInInspector]
    public static RoomUIManager singleton { get; internal set; }

    private TMP_Text ReadyGameButtonText;

    private void Awake()
    {
        InitializeSingleton();
        ReadyGameButtonText = ReadyGameButton.transform.GetComponentInChildren<TMP_Text>();
    }



    public void SetReadyButtonText(bool isReady)
    {
        ReadyGameButtonText.text = isReady ? "Cancel" : "Ready";
    }

    void InitializeSingleton()
    {
        if (singleton != null && singleton == this)
            return;
        singleton = this;
    }

    internal void ShowStartButton()
    {
        StartGameButton.gameObject.SetActive(true);
    }
    internal void HideStartButton()
    {
        StartGameButton.gameObject.SetActive(false);
    }
}
