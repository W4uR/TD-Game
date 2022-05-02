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
    private NetworkManagerTDGame room;
    private NetworkManagerTDGame Room
    {
        get { if (room != null) return room; return room = NetworkManager.singleton as NetworkManagerTDGame; }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Room.Characters.Length; i++)
        {
            var charButton = Instantiate(charaterButtonPrefab, transform);


            CharacterPortraits.Add(charButton.GetComponent<Image>());
            CharacterPortraits[i].sprite = Room.Characters[i].icon;

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
