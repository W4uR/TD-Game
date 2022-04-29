using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LE_InputManager : MonoBehaviour
{

    public static bool MouseOverUI => EventSystem.current.IsPointerOverGameObject();

    public static event Action LeftMouseButton;
    public static event Action RightMouseButton;

    public static event Action E_Button;
    public static event Action T_Button;
    public static event Action<bool> MouseWheel;

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            LeftMouseButton?.Invoke();
        }
        if (Input.GetMouseButton(1))
        {
            RightMouseButton?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            E_Button?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            T_Button?.Invoke();
        }



        if (Input.mouseScrollDelta.y > 0.1f)
        {
            MouseWheel?.Invoke(true);
        }
        else if (Input.mouseScrollDelta.y < -0.1f)
        {
            MouseWheel?.Invoke(false);
        }
    }
}
