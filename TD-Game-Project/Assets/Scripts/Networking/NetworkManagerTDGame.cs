using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkManagerTDGame : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;
    [SerializeField] private string gameSceneName = string.Empty;

    [Header("Room")]
    [SerializeField] private RoomPlayer roomPlayerPrefab = null;
    [Header("Game")]
    [SerializeField] private GamePlayer gamePlayerPrefab = null;
    [SerializeField] private GameObject networkLevelLoader = null;
    public Character[] Characters = new Character[0];

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnectionToClient> OnServerReadied;




    public static byte[] SelectedLevelData;// Only on server

    public List<RoomPlayer> RoomPlayers { get; } = new List<RoomPlayer>();
    public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();
    public RoomPlayer Leader { get; private set; }
    public override void OnStartServer() => spawnPrefabs=Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    public override void OnStartClient(){
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }




    [Client]
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    [Client]
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    [Server]
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
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            RoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

            if (RoomPlayers.Count == 0) //First join
            {
                Leader = roomPlayerInstance;
                roomPlayerInstance.IsLeader = true;
            }

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    [Server]
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<RoomPlayer>();
            RoomPlayers.Remove(player);
            NotifyLeaderOfReadyState();
        }
        
        base.OnServerDisconnect(conn);
    }

    public void NotifyLeaderOfReadyState()
    {
        Leader.HandleReadyToStart(IsReadyToStart());
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

    [Client]
    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    [Server]
    public void StartGame()
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyToStart()) return;

            ServerChangeScene("Map");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        //From Menu to Game
        if (SceneManager.GetActiveScene().path == menuScene && newSceneName == gameSceneName)
        {
            for (int i = RoomPlayers.Count-1;i>=0;i--)
            {
                var conn = RoomPlayers[i].connectionToClient;


                //get spawnpoints from leveldata
                var spawnPoint = LevelLoader.SpawnPointsFromData(SelectedLevelData).GetRandomElement();
                var gameplayerInstance = Instantiate(gamePlayerPrefab,spawnPoint+Vector3.up*i,Quaternion.identity);//,Vector3.up*40f,Quaternion.identity);
                
                //Set infos here
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                gameplayerInstance.SetCharater(RoomPlayers[i].CharacterIndex);


                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);

            }
        }
        
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log(3);
        
        if (sceneName == gameSceneName)
        {
            GameObject nll = Instantiate(networkLevelLoader);
            NetworkServer.Spawn(nll);
        }       
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }

}
