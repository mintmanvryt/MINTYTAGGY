using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using System;

using UnityEngine.XR;

using easyInputs;



public class SwimmySwimmer : MonoBehaviour

{



    private InputDevice LeftDevice;

    private InputDevice RightDevice;



    public Rigidbody GorillaPlayer;

    public float Speed = 1.25f;
    
    public bool IsZeroG = true;
    
    public GameObject[] Effects;

    private bool IsSwimming = false;

    public string TagToDetect = "MainCamera";

    public float intensity;





    private void FixedUpdate()

    {

        float LMag;

        float RMag;

        Vector3 LeftControllerVelocity;

        Vector3 RightControllerVelocity;



        if (IsSwimming)

        {

            LeftDevice.TryGetFeatureValue(CommonUsages.trigger, out var _);

            RightDevice.TryGetFeatureValue(CommonUsages.trigger, out var _);

            LeftDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out LeftControllerVelocity);

            RightDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out RightControllerVelocity);

            LMag = LeftControllerVelocity.magnitude;

            RMag = RightControllerVelocity.magnitude;
if(LMag > 0.7f)
{

}
if(RMag > 0.7f)
{

}

            if (LMag > 2.8f || RMag > 2.8f)

            {

               GorillaPlayer.AddRelativeForce(((-(LeftControllerVelocity + RightControllerVelocity) * Speed) * GorillaPlayer.transform.localScale.y));
                StartCoroutine(EasyInputs.Vibration(EasyHand.RightHand, intensity, 0.2f));
                StartCoroutine(EasyInputs.Vibration(EasyHand.LeftHand, intensity, 0.2f));
            }

        }

    }



    public void LateUpdate()

    {

        List<InputDevice> list = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);

        if (list.Count > 0)

        {

            LeftDevice = list[0];

        }

        List<InputDevice> list2 = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list2);

        if (list2.Count > 0)

        {

            RightDevice = list2[0];

        }

    }



    private void OnTriggerEnter(Collider other)

    {

        if (other.gameObject.CompareTag(TagToDetect))

        {

            IsSwimming = true;

        }
        
        if(IsZeroG)
        {
        	GorillaPlayer.useGravity = false;
        }
        
        foreach(GameObject obj in Effects)
        {
        	if(obj != null)
        	{
        		obj.SetActive(true);
        	}
        }

    }



    private void OnTriggerExit(Collider other)

    {

        if (other.gameObject.CompareTag(TagToDetect))

        {

            IsSwimming = false;

        }
        
        if(IsZeroG)
        {
        	GorillaPlayer.useGravity = true;
        }

        if (other.gameObject.CompareTag(TagToDetect))
        {
            foreach (GameObject obj in Effects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
        

    }

}

