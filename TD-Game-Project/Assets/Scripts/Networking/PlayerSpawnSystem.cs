using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using System;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    private static List<Vector3> spawnPoints = new List<Vector3>();
    private int nextIndex = 0;

    public static void AddSpawnPoint(Vector3 spawnPoint) => spawnPoints.Add(spawnPoint);
    public static void RemoveSpawnPoint(Vector3 spawnPoint) => spawnPoints.Remove(spawnPoint);

    public override void OnStartServer() => NetworkManagerTDGame.OnServerReadied += SpawnPlayer;

    [ServerCallback]
    private void OnDestroy() => NetworkManagerTDGame.OnServerReadied -= SpawnPlayer;

    [Server]
    private void SpawnPlayer(NetworkConnectionToClient conn)
    {

        Vector3 spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex++);

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint,Quaternion.identity);
        NetworkServer.Spawn(playerInstance, conn);
    }


}
