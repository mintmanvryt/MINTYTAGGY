using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PurchaseCosmeticPro : MonoBehaviour
{
    public GameObject ObjectToBuy;
    public GameObject Bought;
    public int Cost;
    public string CurrencyCode;
    public string CatalogName;
    public TextMeshPro Text;
    private string OriginalText;
    private bool CanPress = true;

    private void Start()
    {
        OriginalText = Text.text;
    }

    private void Update()
    {
        if (ObjectToBuy.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            if (Playfablogin.instance.coins >= Cost)
            {
                if (CanPress)
                {
                    CanPress = false;
                    this.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
                    Text.text = "...";
                    PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
                    {
                        ItemId = ObjectToBuy.name,
                        Price = Cost,
                        VirtualCurrency = CurrencyCode,
                        CatalogVersion = CatalogName
                    }, OnPurchaseSuccess, OnPurchaseFailure);
                }
            }
        }
    }

    private void OnPurchaseSuccess(PurchaseItemResult result)
    {
    	StartCoroutine(Get());
    }
    
    IEnumerator Get()
    {
    	
    	Text.text = "Success"; Playfablogin.instance.GetVirtualCurrencies();
    	yield return new WaitForSeconds(1f);
    	ObjectToBuy.SetActive(true);
        Bought.SetActive(true);
    }

    private void OnPurchaseFailure(PlayFabError error)
    {
        StartCoroutine(ErrorP());
        Debug.LogError("Error: " + error.ErrorMessage);
    }
    IEnumerator ErrorP()
    {
    	Text.text = "ERROR";
    	yield return new WaitForSeconds(2f);
    	Text.text = OriginalText;
        this.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
        CanPress = true;		
    }
}
