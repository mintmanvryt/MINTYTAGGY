using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfEnabledList : MonoBehaviour
{
    public GameObject Object;
    public GameObject[] ObjectDisable;

    private void Update()
    {
        if (Object.activeSelf)
        {
            foreach (GameObject Obj in ObjectDisable)
            {
                Obj.SetActive(false);
            }
        }

        else
        {
            foreach (GameObject Obj in ObjectDisable)
            {
                Obj.SetActive(true);
            }
        }
    }
}
