using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using PlayFab;
using PlayFab.ClientModels;

public class joshhsIAPScript : MonoBehaviour
{
    [SerializeField] private string SKU;
    [SerializeField] private string handTag = "HandTag";
    [SerializeField] private bool currency;
    [SerializeField] private bool Object;

    [Space]
    [Header("Only put in a currency amount if you have the currency box checked, same with the object")]
    [SerializeField] private int currencyAmount = 1000;
    [SerializeField] private string currencyCode = "HS";
    [SerializeField] private GameObject TheObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            StartBuy();
        }
    }

    private void StartBuy()
    {
        IAP.LaunchCheckoutFlow(SKU).OnComplete(ObjectBuyCallback);
    }

    private void ObjectBuyCallback(Message<Oculus.Platform.Models.Purchase> msg)
    {
        if (msg.IsError) return;
        StartPurchase();
    }

    private void StartPurchase()
    {
        if (Object)
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = TheObject.name,
                Price = 1,
                VirtualCurrency = "HS",
                CatalogVersion = "Catalog Items",
            }, OnPurchaseSuccess, OnPurchaseFailure);
        }

        if (currency)
        {
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
            {
                Amount = currencyAmount,
                VirtualCurrency = currencyCode
            }, OnAddSucess, OnAddFailure);
        }
    }

    private void OnPurchaseSuccess(PurchaseItemResult result)
    {
        TheObject.SetActive(true);
    }

    private void OnPurchaseFailure(PlayFabError error)
    {
        Debug.LogError("Error" + error.ErrorMessage);
    }

    private void OnAddSucess(ModifyUserVirtualCurrencyResult result)
    {
        Playfablogin.instance.GetVirtualCurrencies();
    }

    private void OnAddFailure(PlayFabError error)
    {
        Debug.LogError("Error" + error.ErrorMessage);
    }
}
