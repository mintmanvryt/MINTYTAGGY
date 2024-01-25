using UnityEngine;
using Photon.VR;

public class BlueColor : MonoBehaviour
{
    public ColorManager colorManager;
    public float blueValue = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            colorManager.currentColor.b = blueValue;
            colorManager.UpdateColor();
        }
    }
}
