using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.VR.Player;
using Photon.VR;

public class TagManager : MonoBehaviourPunCallbacks
{
    
    public bool particles;
    public GameObject tagParticle;
    public List<Renderer> playerParts;
    public Material normalMat;
    public Material tagMat;
    public AudioSource tagSound;
    public TagManager taggedPlayer;
    public PhotonVRPlayer PhotonPlayer;

    public static TagManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (GamemodeManager.instance.isTag)
        {
            if (PhotonNetwork.PlayerList.Length == 1 && !PhotonPlayer.isTagged)
            {
                GetComponent<PhotonView>().RPC("TagPlayer", RpcTarget.AllBufferedViaServer);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (GamemodeManager.instance.isTag)
        {
            if (taggedPlayer == null)
            {
                CheckTagged();
            }
        }
    }
    

    private void CheckTagged()
    {
        TagManager[] currentplayers = TagManager.FindObjectsOfType<TagManager>();
        foreach (TagManager player in currentplayers)
        {
            if (player.PhotonPlayer.isTagged)
            {
                taggedPlayer = player;
            }
        }
        if (taggedPlayer == null)
        {
            foreach (TagManager player in currentplayers)
            {
                if (player.PhotonPlayer.isTagged)
                {
                    taggedPlayer = player;
                }
            }
            if (taggedPlayer == null)
            {
                GetComponent<PhotonView>().RPC("TagMaster", RpcTarget.AllBufferedViaServer);
            }
        }
    }

    [PunRPC]
    public void TagMaster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("TagPlayer", RpcTarget.AllBufferedViaServer);
            }
        }
    }

    [PunRPC]
    public IEnumerator TagPlayer()
    {
        if (GamemodeManager.instance.isTag)
        {
            CheckTagged();
            foreach (Renderer part in playerParts)
            {
                part.material = tagMat;
            }
            if (particles)
            {
                tagParticle.SetActive(true);
            }
            GetComponent<PhotonView>().RPC("PlaySound", RpcTarget.All);
            PhotonPlayer.isTagged = true;
            if (GetComponent<PhotonView>().IsMine)
            {
                GorillaLocomotion.Player.Instance.maxJumpSpeed = 8.5f;
                GorillaLocomotion.Player.Instance.jumpMultiplier = 1.4f;
                GorillaLocomotion.Player.Instance.disableMovement = true;
                yield return new WaitForSeconds(3f);
                GorillaLocomotion.Player.Instance.disableMovement = false;
            }
        }
    }

    [PunRPC]
    public void UntagPlayer()
    {
        foreach (Renderer part in playerParts)
        {
            part.material = normalMat;
        }
        if (particles)
        {
            tagParticle.SetActive(false);
        }
        if (GetComponent<PhotonView>().IsMine)
        {
            GorillaLocomotion.Player.Instance.maxJumpSpeed = 6.8f;
            GorillaLocomotion.Player.Instance.jumpMultiplier = 1.2f;
            PhotonVRManager.Manager.LocalPlayer.RefreshPlayerValues();
        }
        PhotonPlayer.isTagged = false;
    }

    [PunRPC]
    public void PlaySound()
    {
        if (GetComponent <PhotonView>().IsMine)
        {
            tagSound.Play();
        }
    }
}
