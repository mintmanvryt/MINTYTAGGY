using UnityEngine;
using Photon.VR;

public class GreenColor : MonoBehaviour
{
    public ColorManager colorManager;
    public float greenValue = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            colorManager.currentColor.g = greenValue;
            colorManager.UpdateColor();
        }
    }
}
