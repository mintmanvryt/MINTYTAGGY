using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfEnabled : MonoBehaviour
{
    public GameObject Object;
    public GameObject ObjectDisable;

    private void Update()
    {
        if (Object.activeSelf)
        {
            ObjectDisable.SetActive(false);
        }

        else
        {
            ObjectDisable.SetActive(true);
        }
    }
}
