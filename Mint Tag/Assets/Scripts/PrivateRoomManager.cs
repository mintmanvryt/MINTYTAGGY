using Photon.Pun;
using Photon.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateRoomManager : MonoBehaviour
{
    public static PrivateRoomManager Instance;
    public LeaveRoom[] leaveTriggers;
    public bool inPrivateRoom;
    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        
    }
}
