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

    private List<Image> CharacterPortraits = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < myRoomPlayer.Charaters.Length; i++)
        {
            var charButton = Instantiate(charaterButtonPrefab, transform);


            CharacterPortraits.Add(charButton.GetComponent<Image>());
            CharacterPortraits[i].sprite = myRoomPlayer.Charaters[i].icon;

            var index = i;
            charButton.GetComponent<Button>().onClick.AddListener(delegate { SelectCharater(index); });
        }
        SelectCharater(0);
    }

    void SelectCharater(int index)
    {

        myRoomPlayer.CmdSelectCharater(index);

        for (int i = 0; i < CharacterPortraits.Count; i++)
        {
            if(i == index)
            {
                CharacterPortraits[i].color = Color.white;
            }
            else
            {
                CharacterPortraits[i].color = Color.gray;
            }
        }
    }
}
