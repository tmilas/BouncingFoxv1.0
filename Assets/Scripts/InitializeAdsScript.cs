using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

public class InitializeAdsScript : MonoBehaviour,IUnityAdsListener
{

    string gameId = "4589101";
    bool testMode = true;
    bool unityAdReady = false;
    bool googleAdReady = false;

    InterstitialAd googleAd;

    void Start()
    {
        //Unity Ads
        Advertisement.Initialize(gameId, testMode);
        Advertisement.AddListener(this);

        //Google Ads
        MobileAds.Initialize((InitializationStatus obj) => { });
        googleAd = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        googleAd.OnAdLoaded += GoogleAdLoaded;
        googleAd.OnAdDidRecordImpression += GoogleAdRecImp;
        googleAd.OnAdClosed += GoogleAdClosed;

        googleAd.LoadAd(request);

        StartCoroutine(ShowAdsWhenReady());

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
    }

    public void ShowAds()
    {
        //Debug.Log(Advertisement.IsReady("Interstitial_Android"));
        Advertisement.Show("Interstitial_Android");

    }

    public void OnUnityAdsReady(string placementId)
    {
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
       
    }

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
            Advertisement.Show("Interstitial_Android");
        else if (googleAdReady)
            googleAd.Show();
        else
            Debug.Log("Ads Error!!!");
            
    }

}
