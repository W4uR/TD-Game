using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : NetworkRoomManager
{
    [SerializeField]
    [Tooltip("The Room Player Prefab will be Instantiated under this transform")]
    public Transform roomObjectParent;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // increment the index before adding the player, so first player starts at 1
        clientIndex++;

        if (IsSceneActive(RoomScene))
        {
            if (roomSlots.Count == maxConnections)
                return;

            allPlayersReady = false;

            //Debug.Log("NetworkRoomManager.OnServerAddPlayer playerPrefab: {roomPlayerPrefab.name}");

            GameObject newRoomGameObject = OnRoomServerCreateRoomPlayer(conn);
            if (newRoomGameObject == null)
                newRoomGameObject = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity,roomObjectParent);

            NetworkServer.AddPlayerForConnection(conn, newRoomGameObject);
        }
        else
            OnRoomServerAddPlayer(conn);
    }

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        // spawn the initial batch of Rewards
        if (sceneName == GameplayScene)
        {

            foreach (RoomPlayer item in roomSlots)
            {
                Debug.Log(item.charater);
            }
        }
            
    }

}
