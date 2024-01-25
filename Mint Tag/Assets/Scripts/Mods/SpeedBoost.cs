using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GorillaLocomotion;
public class SpeedBoost : MonoBehaviour
{
    public Player GorillaPlayer;
    public float Multiplier;

    void OnTriggerEnter() 
    {
        GorillaPlayer.jumpMultiplier = Multiplier;
    
    }
}
