using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayersInRoom : MonoBehaviour
{
    public TextMeshPro PlayersInRoomText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            PlayersInRoomText.text = playerCount.ToString();
        }
        else
        {
            PlayersInRoomText.text = " ";
        }
    }
}
