using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text Ereaser_text;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void SetIsEreaser(bool isEreser)
    {
        Ereaser_text.text = isEreser ? "Ereaser" : "Brush";
    }
}
