using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera cam = null;
    public override void OnStartAuthority()
    {
        enabled = true;
        cam.enabled = true;
    }
}
