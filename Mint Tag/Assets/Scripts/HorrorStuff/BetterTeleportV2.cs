using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterTeleportV2 : MonoBehaviour
{
    [Header("This script was made by LeXLeo119 Edited by RG_vr. Do not claim it as your own.")]
    public GameObject GorillaPlayer;
    public GameObject TeleportTo;
    public Collider[] CollidersToDisable;
    public float DisableTime;

    public void OnTriggerEnter(Collider other)
    {
        foreach (Collider collider in CollidersToDisable)
        {
            collider.enabled = false;
        }
        
        GorillaPlayer.transform.position = TeleportTo.transform.position;
        
        StartCoroutine(EnableCollidersAfterDelay());
    }

    private IEnumerator EnableCollidersAfterDelay()
    {
        yield return new WaitForSeconds(DisableTime);
        
        foreach (Collider collider in CollidersToDisable)
        {
            collider.enabled = true;
        }
    }
}
