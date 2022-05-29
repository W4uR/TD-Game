using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : NetworkBehaviour
{
    /// <summary>
    /// Units per Second
    /// </summary>
    [SerializeField] private float speed = 10f;
    /// <summary>
    /// Angles per Second
    /// </summary>
    [SerializeField] private float turnSpeed = 1080f;
    [SerializeField] private CharacterController cc = null;
    private Vector2 previousInput;
    private Controls controls = null;
    private Controls Controls
    {
        get
        {
            if (controls != null) return controls;
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        cc.enabled = true;
        enabled = true;
        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => ResetMovemet();
    }




    [ClientCallback]
    private void OnEnable() { Controls.Enable();}
    [ClientCallback]
    private void OnDisable() {Controls.Disable();}


    [Client]
    private void ResetMovemet() => previousInput = Vector2.zero;
    [Client]
    private void SetMovement(Vector2 moveInput) => previousInput = moveInput;

    [ClientCallback]
    private void Update() => Move();

    private void Move()
    {
        if (!cc.isGrounded) cc.Move(Util.GRAVITY*Time.deltaTime);
        if (previousInput == Vector2.zero) return;
        var rot = Quaternion.LookRotation(previousInput.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        cc.Move(previousInput.ToIso() * Time.deltaTime * speed); //brb
    }

}
