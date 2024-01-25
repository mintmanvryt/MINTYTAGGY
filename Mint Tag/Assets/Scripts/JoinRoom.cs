using UnityEngine;
using Photon.VR;
using Photon.Pun;
using System.Collections;

public class JoinRoom : MonoBehaviour
{
    public RoomScript roomScript;

    public string Handtag;


    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Handtag))
        {
            int maxPlayers = 10;
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                yield return new WaitForSeconds(2f);
            }
            string room = roomScript.RoomVar + SwitchGamemode.instance.currentGamemode;
            PhotonVRManager.JoinPrivateRoom(room, maxPlayers);
        }
    }
}