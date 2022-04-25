using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayer : NetworkRoomPlayer
{
    [Tooltip("The charater selected by the player")]
    [SyncVar(hook = nameof(CharaterChanged))]
    public Charater charater = 0;




    public void SelectCharater()
    {

    }

    [Command]
    public void CmdChangeCharater()
    {
        
    }

    public void CharaterChanged(Charater oldC, Charater newC)
    {
        
    }

}
