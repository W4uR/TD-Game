using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Camera cam;
    public MapEditor me;

    private void Awake()
    {
        cam = Camera.main;

    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            me.OnLeftMouseBtn();

        }else if (Input.GetMouseButton(1))
        {
            me.OnRightMouseBtn();
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            me.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            me.Load();
        }



    }
}
