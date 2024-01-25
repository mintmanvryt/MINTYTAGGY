using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;

public class GameObjectShopManager : MonoBehaviour
{
    [SerializeField] private string skuToPurchase;
    [SerializeField] private GameObject purchaseableObject;
    [SerializeField] private string NameOfObjectToBuy;
    public GameObject Enable;

    private string[] skus = { "cube-001", "cube-002" };

    private void Update()
    {
        if (purchaseableObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }

        if (Enable.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        GetPrices();
        GetPurchases();

        if (PlayerPrefs.HasKey(NameOfObjectToBuy))
        {
            bool isPurchased = PlayerPrefs.GetInt(NameOfObjectToBuy) == 1;
            purchaseableObject.SetActive(isPurchased);
        }
    }

    private void GetPrices()
    {
        IAP.GetProductsBySKU(skus).OnComplete(GetPricesCallback);
    }

    private void GetPricesCallback(Message<ProductList> msg)
    {
        if (msg.IsError) return;

    }

    private void GetPurchases()
    {
        IAP.GetViewerPurchases().OnComplete(GetPurchasesCallback);
    }

    private void GetPurchasesCallback(Message<PurchaseList> msg)
    {
        if (msg.IsError) return;
    }

    public void BuyProduct()
    {
        IAP.LaunchCheckoutFlow(skuToPurchase).OnComplete(BuyProductCallback);
    }

    private void BuyProductCallback(Message<Oculus.Platform.Models.Purchase> msg)
    {
        if (msg.IsError) return;

        purchaseableObject.SetActive(true);

        PlayerPrefs.SetInt(NameOfObjectToBuy, 1);
        PlayerPrefs.Save();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            BuyProduct();
        }
    }
}