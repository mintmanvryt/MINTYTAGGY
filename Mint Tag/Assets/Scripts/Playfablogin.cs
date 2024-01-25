using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using System.Threading.Tasks;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System.Globalization;
using System;
using TMPro;
using Photon.VR;
using System.Net;
using Photon.Realtime;
using Photon.Voice;
using UnityEngine.Animations;
using Unity.XR.Oculus;
using UnityEngine.Networking;
using Photon.Pun;



public class Playfablogin : MonoBehaviour

{
    [Serializable]
    public class PlayFabItems
    {
        public string ObjectName;
        public GameObject Object;
        public bool IsOwned;
    }

    [Header("COSMETICS")]

    public static Playfablogin instance;

    [NonSerialized] public string MyPlayFabID;

    public string CatalogName;

    [SerializeField] private List<PlayFabItems> Specialitems;
    public List<PlayFabItems> specialitems
    {
        get
        {
            return Specialitems;
        }
    }

    [SerializeField] private List<PlayFabItems> Disableitems;
    public List<PlayFabItems> disableitems
    {
        get
        {
            return Disableitems;
        }
    }

    [Header("CURRENCY")]

    public string CurrencyCode;

    public string CurrencyName;

    public TextMeshPro[] currencyText;

    [NonSerialized] public int coins;

    [Header("BAN STUFF")]

    public GameObject[] StuffToDisable;

    public GameObject[] StuffToEnable;

    public MeshRenderer[] StuffToMaterialChange;

    public Material MaterialToChangeToo;

    public TextMeshPro[] BanTimes;

    public TextMeshPro[] BanReasons;

    [Header("TITLE DATA")]

    public TextMeshPro MOTDText;
    public TextMeshPro CREDText;

    [Header("OTHER")]
    public List<TextMeshPro> IdDisplays;

    [Header("AUTH")]

    public PhotonVRManager photonvr;

    [NonSerialized]

    private bool IsLoggedIn = false;
    private bool Banned = false;
    public bool IsBanned
    {
        get
        {
            return Banned;
        }
    }
    public bool LoggedIn
    {
        get
        {
            return IsLoggedIn;
        }
    }









    public void Awake()

    {

        instance = this;

    }



    void Start()

    {
        if (PlayFabSettings.TitleId == "fuckyou")
        {
            PlayFabSettings.TitleId = "5273E";
        }
        if (PlayFabSettings.TitleId == "5273E")
        {

            login();

        }
        else
        {

            Debug.Log("Wrong PlayFab Title ID!");
            Application.Quit();

        }

        InvokeRepeating("CheckBanned", 15f, 15f);

    }

    private void OnApplicationQuit()
    {
        PlayFabSettings.TitleId = "fuckyou";
    }



    private void CheckBanned()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, OnGetAccountInfoError);
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)

    {



    }



    private void OnGetAccountInfoError(PlayFabError error)

    {

        if (error.Error == PlayFabErrorCode.AccountBanned && Banned == false)

        {

            PhotonVRManager.Manager.Disconnect();

            Application.Quit();

        }

    }

    public void login()

    {



        var request = new LoginWithCustomIDRequest

        {

            CustomId = SystemInfo.deviceUniqueIdentifier,

            CreateAccount = true,

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams

            {

                GetPlayerProfile = true

            }

        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);

    }



    public void OnLoginSuccess(LoginResult result)

    {

        Debug.Log("logging in");

        GetAccountInfoRequest InfoRequest = new GetAccountInfoRequest();

        string NameChangeTo = PlayerPrefs.GetString("username");

        var request = new UpdateUserTitleDisplayNameRequest()
        {

            DisplayName = NameChangeTo,

        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, UpdatedName, ErrorUpdatingName);

        PlayFabClientAPI.GetAccountInfo(InfoRequest, AccountInfoSuccess, OnError);

        GetVirtualCurrencies();

        GetMOTD();

        IsLoggedIn = true;

    }

    void UpdatedName(UpdateUserTitleDisplayNameResult result)
    {

        Debug.Log("Updated Playfab Display Name");

    }

    void ErrorUpdatingName(PlayFabError error)
    {

        Debug.Log("Error updating Playfab Display Name!");

    }



    public void AccountInfoSuccess(GetAccountInfoResult result)

    {

        MyPlayFabID = result.AccountInfo.PlayFabId;
        PlayerPrefs.SetString("PlayFabPlayerID", MyPlayFabID);
        foreach (TextMeshPro i in IdDisplays)
        {
            i.text = "PlayFab ID: " + MyPlayFabID.ToString();
        }

        string NameChangeTo = PlayerPrefs.GetString("username");

        RequestPhotonToken();



        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),

        (result) =>

        {

            foreach (var item in result.Inventory)

            {

                if (item.CatalogVersion == CatalogName)

                {

                    for (int i = 0; i < Specialitems.Count; i++)

                    {

                        if (Specialitems[i].Object.name == item.ItemId)

                        {

                            Specialitems[i].Object.SetActive(true);
                            Specialitems[i].IsOwned = true;

                        }

                    }

                    for (int i = 0; i < Disableitems.Count; i++)

                    {

                        if (Disableitems[i].Object.name == item.ItemId)

                        {

                            Disableitems[i].Object.SetActive(false);
                            Disableitems[i].IsOwned = true;

                        }

                    }

                }

            }

        },

        (error) =>

        {

            Debug.LogError(error.GenerateErrorReport());

        });

        for (int j = 0; j < Specialitems.Count; j++)
        {
            if (Specialitems[j].IsOwned == false)
            {
                Specialitems[j].Object.SetActive(false);
            }
        }
        for (int a = 0; a < Disableitems.Count; a++)
        {
            if (Disableitems[a].IsOwned == false)
            {
                Disableitems[a].Object.SetActive(true);
            }
        }

    }



    public void GetVirtualCurrencies()

    {

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);

    }



    void OnGetUserInventorySuccess(GetUserInventoryResult result)

    {

        coins = result.VirtualCurrency[CurrencyCode];
        foreach (var currencytxt in currencyText)
        {
            currencytxt.text = "You have: " + coins.ToString() + " " + CurrencyName;
        }

    }



    private void OnError(PlayFabError error)

    {

        if (error.Error == PlayFabErrorCode.AccountBanned)

        {
            CancelInvoke("CheckBanned");
            Banned = true;

            PhotonVRManager.Manager.Disconnect();

            foreach (GameObject obj in StuffToDisable)

            {

                obj.SetActive(false);

            }

            foreach (GameObject obj in StuffToEnable)

            {

                obj.SetActive(true);

            }

            foreach (MeshRenderer rend in StuffToMaterialChange)

            {

                rend.material = MaterialToChangeToo;

            }

            foreach (var item in error.ErrorDetails)

            {

                foreach (TextMeshPro BanTime in BanTimes)

                {

                    if (item.Value[0] == "Indefinite")

                    {

                        BanTime.text = "Permanent Ban";

                    }

                    else

                    {

                        string playFabTime = item.Value[0];

                        DateTime unityTime;

                        try

                        {

                            unityTime = DateTime.ParseExact(playFabTime, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);

                            TimeSpan timeLeft = unityTime.Subtract(DateTime.UtcNow);

                            int hoursLeft = (int)timeLeft.TotalHours;

                            hoursLeft = hoursLeft + 1;

                            BanTime.text = string.Format("Hours Left: {0}", hoursLeft);

                        }

                        catch (FormatException ex)

                        {

                            Debug.LogErrorFormat("Failed to parse PlayFab time '{0}': {1}", playFabTime, ex.Message);

                        }

                    }

                }

                foreach (TextMeshPro BanReason in BanReasons)

                {

                    BanReason.text = string.Format("Reason: {0}", item.Key);

                }

            }

        }

        else

        {
            if (PlayFabSettings.TitleId == "fuckyou")
            {
                PlayFabSettings.TitleId = "5273E";
            }
            if (PlayFabSettings.TitleId == "5273E")
            {
                login();
            }
            else
            {
                Debug.Log("Wrong Title ID!");
                Application.Quit();
            }

        }

    }



    public void GetMOTD()

    {

        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), MOTDGot, OnError);

    }



    public void MOTDGot(GetTitleDataResult result)

    {

        if (result.Data == null || result.Data.ContainsKey("MOTD") == false)

        {

            Debug.Log("No MOTD");

        }
        else
        {



            MOTDText.text = result.Data["MOTD"];
        }
        if (result.Data == null || result.Data.ContainsKey("CRED") == false)

        {

            Debug.Log("No CRED");

        }
        else
        {



            CREDText.text = result.Data["CRED"];
        }
        if (result.Data == null || result.Data.ContainsKey("VER") == false)

        {

            Debug.Log("No VER");

        }
        else
        {



            if (Application.version != result.Data["VER"])
            {
                Application.Quit();
            }
        }




    }

    //AUTH PUN ----------------------------------------------------------------------------------------------------------------------------

    public void RequestPhotonToken()
    {
        //var request = new GetPhotonAuthenticationTokenRequest
        //{
        //PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = photonvr.AppId,
        //};

        //PlayFabClientAPI.GetPhotonAuthenticationToken(request, OnPhotonTokenSuccess, OnPhotonTokenFailure);

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = photonvr.AppId,
        }, OnPhotonTokenSuccess, OnPhotonTokenError);

    }

    private void OnPhotonTokenSuccess(GetPhotonAuthenticationTokenResult result)
    {
        //result.PhotonCustomAuthenticationToken
        string customauthtoken = result.PhotonCustomAuthenticationToken;

        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.AddAuthParameter("username", MyPlayFabID);
        PhotonNetwork.AuthValues.AddAuthParameter("token", customauthtoken);

        Debug.Log("Authenticating..");


        Debug.Log("Authentication successful.");

        PhotonNetwork.ConnectUsingSettings();

    }

    private void OnPhotonTokenError(PlayFabError error)
    {
        Debug.Log("GetPhotonAuthenticationToken failed: " + error.GenerateErrorReport());

        Debug.Log("Lmao imagine trying to get the servers");


    }

}