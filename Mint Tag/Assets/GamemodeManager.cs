using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeManager : MonoBehaviour
{
    public static GamemodeManager instance;
    public string currentGamemode;
    public bool isTag;
    public bool isCasual;
    public bool isPvp;
    public bool isHammer;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isTag)
        {
            currentGamemode = "TAG";
        }
        if (isCasual)
        {
            currentGamemode = "CASUAL";
        }
        if (isPvp)
        {
            currentGamemode = "PVP";
        }
        if (isHammer)
        {
            currentGamemode = "HAMMER";
        }
    }
}
