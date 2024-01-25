using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.VR;
using Photon.VR.Cosmetics;
using TMPro;
using easyInputs;

public class CosmeticButton : MonoBehaviour
{
    private Renderer ButtonObject;
    public TextMeshPro TMPTextObject;
    public string NameOfCosmetic;
    private string Empty;
    private string EquipText;
    private string UnequipText;
    public CosmeticType Type;
    private Color RedMaterial = Color.red;
    private Color WhiteMaterial = Color.white;
    public AudioSource click;

    private bool hasBeenTouched = false;

    private IEnumerator Start()
    {
        ButtonObject = GetComponent<Renderer>();
        EquipText = "Equip";
        UnequipText = "Unequip";
        WhiteMaterial = ButtonObject.material.color;
        click = GameObject.FindWithTag("ClickSound").GetComponent<AudioSource>();

        yield return new WaitForSeconds(1);
        //cosmetic saving
        if (PlayerPrefs.GetString(Type.ToString() + "Button") == name)
        {
            hasBeenTouched = true;
            ButtonObject.material.color = RedMaterial;
            TMPTextObject.text = UnequipText;
        }
        else
        {
            hasBeenTouched = false;
            ButtonObject.material.color = WhiteMaterial;
            TMPTextObject.text = EquipText;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HandColliders>().isLeftHand)
        {
            click.GetComponent<Transform>().position = GetComponent<Transform>().position;
            StartCoroutine(EasyInputs.Vibration(EasyHand.LeftHand, 0.15f, 0.15f));
            click.Play();
            if (!hasBeenTouched)
            {
                PhotonVRManager.SetCosmetic(Type, NameOfCosmetic);
                hasBeenTouched = true;
                ButtonObject.material.color = RedMaterial;
                TMPTextObject.text = UnequipText;
                PlayerPrefs.SetString(Type.ToString() + "Button", name);
            }
            else
            {
                PhotonVRManager.SetCosmetic(Type, Empty);
                hasBeenTouched = false;
                ButtonObject.material.color = WhiteMaterial;
                TMPTextObject.text = EquipText;
            }
        }
        if (other.GetComponent<HandColliders>().isRightHand)
        {
            click.GetComponent<Transform>().position = GetComponent<Transform>().position;
            StartCoroutine(EasyInputs.Vibration(EasyHand.RightHand, 0.15f, 0.15f));
            click.Play();
            if (!hasBeenTouched)
            {
                PhotonVRManager.SetCosmetic(Type, NameOfCosmetic);
                hasBeenTouched = true;
                ButtonObject.material.color = RedMaterial;
                TMPTextObject.text = UnequipText;
            }
            else
            {
                PhotonVRManager.SetCosmetic(Type, Empty);
                hasBeenTouched = false;
                ButtonObject.material.color = WhiteMaterial;
                TMPTextObject.text = EquipText;
            }
        }
    }
} 
