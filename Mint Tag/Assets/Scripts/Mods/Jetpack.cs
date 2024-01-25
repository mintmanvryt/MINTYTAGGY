using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class Jetpack : MonoBehaviour
{
    public Rigidbody GorillaPlayer;

    public int Xforce;
    public int Yforce;
    public int Zforce;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
        {
            GorillaPlayer.AddForce(Xforce, Yforce, Zforce, ForceMode.Acceleration);
        }


    }
}