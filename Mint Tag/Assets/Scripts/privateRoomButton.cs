using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class privateRoomButton : MonoBehaviour
{
    public PrivateRoomManager roomManager;
    public bool leave;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            if (leave)
            {
                foreach (LeaveRoom g in roomManager.leaveTriggers)
                {
                    g.gameObject.SetActive(false);
                }
                roomManager.inPrivateRoom = true;
            }
            else
            {
                foreach (LeaveRoom g in roomManager.leaveTriggers)
                {
                    g.gameObject.SetActive(true);
                }
                roomManager.inPrivateRoom = false;
            }
        }
    }
}
