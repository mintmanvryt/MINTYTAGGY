using easyInputs;
using Photon.VR;
using Photon.VR.Cosmetics;
using UnityEngine;

public class InvisMod : MonoBehaviour
{
    public EasyHand hand;
    public string cosmeticName;

    private void Update()
    {
        if (EasyInputs.GetTriggerButtonDown(hand) && EasyInputs.GetGripButtonDown(hand) && EasyInputs.GetPrimaryButtonDown(hand))
        {
            PhotonVRManager.SetCosmetic(CosmeticType.Body, cosmeticName);
        }
        else
        {
            if (PhotonVRManager.Manager.Cosmetics.Body == cosmeticName)
            {
                PhotonVRManager.SetCosmetic(CosmeticType.Body, "");
            }
        }
    }
}
