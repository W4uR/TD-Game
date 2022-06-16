using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LE_CameraController : MonoBehaviour
{

    Camera cam;

    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;
    private bool drag;

    private void Awake()
    {
        cam = Camera.main;
        ResetCamera = cam.transform.position;
    }

    private void OnEnable()
    {
        LE_InputManager.MouseWheel += Zoom;
    }
    private void OnDisable()
    {
        LE_InputManager.MouseWheel -= Zoom;
    }


    private void Zoom(bool into)
    {
        if (Input.GetKey(KeyCode.LeftControl) == false) return;

        if (into)
        {

            if (cam.orthographicSize > 20f)
            {
                cam.orthographicSize -= 4.7f;
            }
        }
        else
        {
            if (cam.orthographicSize < 200)
            {
                cam.orthographicSize += 4.7f;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.LeftControl) == false) return;
        if (Input.GetMouseButton(1))
        {
            Difference = (cam.ScreenToWorldPoint(Input.mousePosition)) - cam.transform.position;
            if (drag == false)
            {
                drag = true;
                Origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }
        if (drag)
        {
            cam.transform.position = Origin - Difference;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            cam.transform.position = ResetCamera;
        }

    }
}
