using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModMenuButton : MonoBehaviour
{
    private Color ButtonStartColor;
    public Color ButtonPressedColor;

    public GameObject mod;

    private bool on;

    private Renderer buttonRenderer;

    void Start()
    {
        buttonRenderer = this.gameObject.GetComponent<Renderer>();
        ButtonStartColor = buttonRenderer.material.color;
        mod.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            changebutton();
        }
    }
    void changebutton()
    {
        if (on)
        {
            buttonRenderer.material.color = ButtonStartColor;
            mod.SetActive(false);
            on = false;
        }
        else
        {
            buttonRenderer.material.color = ButtonPressedColor;
            mod.SetActive(true);
            on = true;
        }
    }
}
