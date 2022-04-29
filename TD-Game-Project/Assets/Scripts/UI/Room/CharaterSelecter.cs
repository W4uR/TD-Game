using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterSelecter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RoomPlayer myRoomPlayer = null;
    [SerializeField] GameObject charaterButtonPrefab = null;

    private List<Image> CharaterImages = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < myRoomPlayer.Charaters.Length; i++)
        {
            var charButton = Instantiate(charaterButtonPrefab, transform);


            CharaterImages.Add(charButton.GetComponent<Image>());
            CharaterImages[i].sprite = myRoomPlayer.Charaters[i].icon;

            var index = i;
            charButton.GetComponent<Button>().onClick.AddListener(delegate { SelectCharater(index); });
        }
        SelectCharater(0);
    }

    void SelectCharater(int index)
    {
        Debug.Log("CLICKED BUTTON  " + index);
        myRoomPlayer.CmdSelectCharater(index);

        for (int i = 0; i < CharaterImages.Count; i++)
        {
            if(i == index)
            {
                CharaterImages[i].color = Color.white;
            }
            else
            {
                CharaterImages[i].color = Color.gray;
            }
        }
    }
}
