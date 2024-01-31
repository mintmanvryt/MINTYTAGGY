using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class NetworkedHitsounds : MonoBehaviour
{
    [Header("Made by joshh, do not share.")]
    public bool left;
    public Transform controller;
    public float vibrationIntens = 0.15f;
    public HitsoundData data;

    private void OnTriggerEnter(Collider other)
    {
        if (left)
        {
            StartCoroutine(EasyInputs.Vibration(EasyHand.LeftHand, vibrationIntens, 0.15f));

        }
        else
        {
            StartCoroutine(EasyInputs.Vibration(EasyHand.RightHand, vibrationIntens, 0.15f));

        }
        if (PhotonNetwork.InRoom)
        {
            foreach (HitsoundData.hitsoundssettings hs in data.hitSounds)
            {
                if (other.CompareTag(hs.objectTag))
                {
                    GameObject temp = PhotonNetwork.Instantiate(hs.audioLocation, controller.position, controller.rotation);
                    StartCoroutine(destroy(temp));
                }
            }
        }
        else
        {
            foreach (HitsoundData.hitsoundssettings hs in data.hitSounds)
            {
                if (other.CompareTag(hs.objectTag))
                {
                    GameObject temp = Instantiate(hs.audioPrefab, controller.position, controller.rotation);
                    StartCoroutine(destroy(temp));
                }
            }
        }
    }

    IEnumerator destroy(GameObject o)
    {
        yield return new WaitForSeconds(o.GetComponent<AudioSource>().clip.length);
        if (PhotonNetwork.InRoom)
        {

            PhotonNetwork.Destroy(o);
        }
        else
        {
            destroy(o);
        }
    }

}
