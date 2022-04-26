using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRoomManager : NetworkManager
{
    [Scene][SerializeField] private string menuScene;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = null;
    [SerializeField] private byte minPlayers = 2;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();

    private NetworkConnection leader;
    public NetworkConnection Leader { get => leader; private set => leader = value; }

    public override void OnStartServer() => spawnPrefabs.AddRange(Resources.LoadAll<GameObject>("SpawnablePrefabs"));
    public override void OnStartClient() => spawnPrefabs.AddRange(Resources.LoadAll<GameObject>("SpawnablePrefabs"));

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }
        if (SceneManager.GetActiveScene().path != menuScene) //We are already in-game
        {
            conn.Disconnect();
            return;
        }
    }

    [Server]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log(SceneManager.GetActiveScene().path + "   " + menuScene);
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;
            Debug.Log("Ide is eljutottam");

            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab, RoomUIManager.singleton.RoomPlayerParent);
            
            if (isLeader)
            {
                roomPlayerInstance.SetAsLeader();
                Leader = conn;
            }

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }



    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayer>();
            RoomPlayers.Remove(player);
            NotifyLeaderOfReadyState();
        }
        base.OnServerDisconnect(conn);
    }
    [Server]
    public void NotifyLeaderOfReadyState()
    {
        leader.identity.GetComponent<NetworkRoomPlayer>().HandleReadyToStart(IsReadyToStart());
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) return false;

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) return false;
        }
        return true;
    }




    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }
}
