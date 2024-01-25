using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class modmenu : MonoBehaviour
{
    // Public

    [Header("SCRIPT BY LUCKMONK. DO NOT CLAIM AS OWN.")]
    [Header("MUST HAVE EASYINPUTS BY KINEXDEV INSTALLED.")]

    [Header("Mod Menu Parent")]
    public GameObject ModMenuParent;

    [Header("Hand")]
    public EasyHand Hand;

    // Private

    void Start()
    {
        ModMenuParent.SetActive(false);
    }

    void Update()
    {
        if (EasyInputs.GetPrimaryButtonDown(Hand))
        {
            ModMenuParent.SetActive(true);
        }

        else
        {
            ModMenuParent.SetActive(false);
        }
    }
}