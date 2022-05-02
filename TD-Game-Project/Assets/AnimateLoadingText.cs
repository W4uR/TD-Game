using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AnimateLoadingText : MonoBehaviour
{
    [SerializeField] TMP_Text loadingText = null;

    [Range(0.1f,6f)]
    [SerializeField] float AnimationSpeed = 1f;
    private float speed;

    private float timer = 0f;
    private int counter = 3;

    const string LOADING = "Loading...";
    private void OnValidate()
    {
        speed = 1f / AnimationSpeed;
    }
    private void FixedUpdate()
    {
        //if (gameObject.activeSelf == false) return;
        if (timer- speed >= 0f)
        {
            timer = 0;
            counter++;
            loadingText.text = LOADING.Substring(0, 7 + counter%4);
        }

        timer += Time.fixedDeltaTime;
    }
}
