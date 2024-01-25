using Photon.Pun;
using UnityEngine;

public class TagHand : MonoBehaviour
{
    public string ColliderTag = "TagCollider";
    public PhotonView view;
    public bool canTag;

    public static TagHand instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (view == null)
            {
                view = GameObject.FindWithTag(ColliderTag).GetComponent<PhotonView>();
            }
            if (view.GetComponent<TagManager>().PhotonPlayer.isTagged)
            {
                canTag = true;
            }
            else
            {
                canTag = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ColliderTag)
        {
            if (other.GetComponent<PhotonView>().IsMine == false && canTag)
            {
                if (other.GetComponent<TagManager>())
                {
                    other.GetComponent<PhotonView>().RPC("TagPlayer", RpcTarget.All);
                }
                if (view.GetComponent<TagManager>())
                {
                    view.GetComponent<PhotonView>().RPC("UntagPlayer", RpcTarget.All);
                }
            }
        }
    }
}
