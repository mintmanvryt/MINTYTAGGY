using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitsoundData", menuName = "UltraAssets/Hitsounds")]
public class HitsoundData : ScriptableObject
{
    [Serializable]
    public struct hitsoundssettings
    {
        public string audioLocation;
        public string objectTag;
        public GameObject audioPrefab;
    }
    [SerializeField]
    public List<hitsoundssettings> hitSounds;

}
