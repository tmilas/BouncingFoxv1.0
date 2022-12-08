using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

public class InitializeAdsScript : MonoBehaviour,IUnityAdsLoadListener,IUnityAdsShowListener,IUnityAdsInitializationListener
{

    string gameIdAndroid = "4589101";
    string gameIdIOS = "4589100";

    bool testMode = true;
    bool unityAdReady = false;
    bool googleAdReady = false;

    InterstitialAd googleAd;
    string _adUnitId;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? gameIdIOS
            : gameIdAndroid;

        _adUnitId = gameIdAndroid;
        Debug.Log("Platform:" + Application.platform);
    }

    void Start()
    {

    }
    
    public void ShowAds()
    {
        //Unity Ads
        //Advertisement.Initialize(gameId, testMode);
        //Advertisement.AddListener(this);
        Advertisement.Initialize(_adUnitId, true, this);


        //Google Ads
        MobileAds.Initialize((InitializationStatus obj) => { });
        googleAd = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
        googleAd.OnAdLoaded += GoogleAdLoaded;
        googleAd.OnAdDidRecordImpression += GoogleAdRecImp;
        googleAd.OnAdClosed += GoogleAdClosed;
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        googleAd.LoadAd(request); 

        StartCoroutine(ShowAdsWhenReady());

        Debug.Log(SystemInfo.deviceUniqueIdentifier);

    }

    private void GoogleAdLoaded(object sender, System.EventArgs e)
    {
        googleAdReady = true;
    }

    private void GoogleAdRecImp(object sender, System.EventArgs e)
    {
        Debug.Log("Google Did Record Impression");
    }

    private void GoogleAdClosed(object sender, System.EventArgs e)
    {
        Debug.Log("Google Ad Closed");
        GameEngine gameEngine = FindObjectOfType<GameEngine>();
        gameEngine.ContinueGame();

    }

    public void ShowUnityTestAds()
    {
        //Debug.Log(Advertisement.IsReady("Interstitial_Android"));
        Advertisement.Show("Interstitial_Android");

    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log(placementId);
        if (placementId.Equals("Interstitial_Android"))
            unityAdReady = true;
        //Debug.Log(placementId);
        //Debug.Log(Advertisement.IsReady(placementId));
    }

    public void OnUnityAdsDidError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ads Start");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Ads Finish");
        GameEngine gameEngine = FindObjectOfType<GameEngine>();
        gameEngine.ContinueGame();


    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("ads loaded");
        unityAdReady = true;

        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }

    IEnumerator ShowAdsWhenReady()
    {
        /*while (!Advertisement.IsReady("Interstitial_Android") && !googleAd.IsLoaded())
        {
            yield return new WaitForSeconds(0.5f);
        }*/
        while (!unityAdReady && !googleAdReady)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (unityAdReady)
            //Advertisement.Show("Interstitial_Android");
            Advertisement.Show("Interstitial_Android", this);
        else if (googleAdReady)
            googleAd.Show();
        else
            Debug.Log("Ads Error!!!");
            
    }

    void IUnityAdsInitializationListener.OnInitializationComplete()
    {
        Advertisement.Load("Interstitial_Android", this);

        //throw new System.NotImplementedException();
    }

    void IUnityAdsInitializationListener.OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        throw new System.NotImplementedException();
    }
}
