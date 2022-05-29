using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using System;

public class NetworkLevelLoader : NetworkBehaviour
{

    public override void OnStartServer() => NetworkManagerTDGame.OnServerReadied += LoadLevelForPlayer;

    [ServerCallback]
    private void OnDestroy() => NetworkManagerTDGame.OnServerReadied -= LoadLevelForPlayer;

    [Server]
    private void LoadLevelForPlayer(NetworkConnectionToClient conn)
    {
        //Now the server reloads the level for every loaded player...this is not good
        LevelLoader.Singleton.LoadLevel(NetworkManagerTDGame.SelectedLevelData);
        LoadLevelTRPC(conn, NetworkManagerTDGame.SelectedLevelData);
        //conn.identity.transform.position = LevelLoader.GetRandomSpawnPoint();
        //Debug.Break();
    }
    [TargetRpc]
    void LoadLevelTRPC(NetworkConnection conn, byte[] levelData)
    {
        if (isServer) return;
        LevelLoader.Singleton.LoadLevel(levelData);
    }

}