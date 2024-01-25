using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.VR;
using Photon.Pun;

public class JoinPub : MonoBehaviour
{
    public string Queue = "Default";

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            PhotonVRManager.JoinRandomRoom(Queue);
        }
    }
}
