using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGamemode : MonoBehaviour
{
    public bool TagButton;
    public bool CasualButton;
    public bool MinigamesTag;
    public bool MinigamesCasual;
    private SwitchGamemode[] buttons;
    private Renderer button;
    public string currentGamemode;

    public GameObject[] old;
    public GameObject New;

    public static SwitchGamemode instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buttons = FindObjectsOfType<SwitchGamemode>();
        button = GetComponent<Renderer>();
        if (TagButton)
        {
            button.material.color = Color.red;
        }
        else
        {
            button.material.color = Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            foreach (var button in buttons)
            {
                button.button.material.color = Color.white;
            }
            button.material.color = Color.red;
            foreach (GameObject o in old)
            {
                o.SetActive(false);
            }
            New.SetActive(true);

            if (TagButton)
            {
                GamemodeManager.instance.isTag = true;
                GamemodeManager.instance.isCasual = false;
                currentGamemode = "TAG";
            }
            if (CasualButton)
            {
                GamemodeManager.instance.isCasual = true;
                GamemodeManager.instance.isTag = false;
                currentGamemode = "CASUAL";
            }
            if (MinigamesTag)
            {
                GamemodeManager.instance.isCasual = false;
                GamemodeManager.instance.isTag = true;
                currentGamemode = "MINIGAMES";
            }
            if (MinigamesCasual)
            {
                GamemodeManager.instance.isCasual = true;
                GamemodeManager.instance.isTag = false;
                currentGamemode = "PVP";
            }
        }
    }
}