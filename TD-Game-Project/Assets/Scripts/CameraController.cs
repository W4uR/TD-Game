using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
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

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
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


        if (Input.mouseScrollDelta.y > 0.1f)
        {

            if (cam.orthographicSize > 3.5f)
            {
                cam.orthographicSize -= 0.5f;
            }
        }
        else if (Input.mouseScrollDelta.y < -0.1f)
        {
            if (cam.orthographicSize < 25.5f)
            {
                cam.orthographicSize += 0.5f;
            }
        }
    }
}
