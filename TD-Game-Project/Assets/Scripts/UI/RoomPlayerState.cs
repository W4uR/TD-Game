using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomPlayerState
{
    public int ClientId { get; internal set; }
    public string PlayerName { get; internal set; }
    public int CharaterIndex { get; internal set; }
    public bool IsReady { get; internal set; }

    public RoomPlayerState(int clientId, string playerName, int charaterIndex, bool isReady)
    {
        ClientId = clientId;
        PlayerName = playerName;
        CharaterIndex = charaterIndex;
        IsReady = isReady;
    }

}
