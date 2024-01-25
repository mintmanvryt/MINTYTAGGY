using UnityEngine;
using Photon.VR;

public class RedColor : MonoBehaviour
{
    public ColorManager colorManager;
    public float redValue = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            colorManager.currentColor.r = redValue;
            colorManager.UpdateColor();
        }
    }
}
