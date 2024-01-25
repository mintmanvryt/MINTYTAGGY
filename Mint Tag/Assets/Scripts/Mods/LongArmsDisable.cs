using UnityEngine;

public class LongArmsDisable : MonoBehaviour
{
    [SerializeField] private string handTag = "HandTag";
    [SerializeField] private GameObject gorillaRig;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            gorillaRig.transform.localScale = Vector3.one; 
        }
    }
}
