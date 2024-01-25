using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolldown : MonoBehaviour
{
public List<GameObject> Tabs = new List<GameObject>();
public Scrollup OpposingButton;
public int CurrentTab = 1;

public void OnTriggerEnter(Collider other)
{
if(other.tag == "Handtag");

{

GameObject NewTab = Tabs[CurrentTab + 1];
GameObject OldTab = Tabs[CurrentTab];
NewTab.SetActive(true);
OldTab.SetActive(false);
CurrentTab = CurrentTab + 1;
OpposingButton.CurrentTab = CurrentTab;

}


}

}
