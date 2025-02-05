using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class interstitialAdManager : MonoBehaviour
{

    #if UNITY_ANDROID
  private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif

    public bool runAd;

    
    
  // These ad units are configured to always serve test ads.


  private InterstitialAd _interstitialAd;

  void Start() {
        MobileAds.Initialize(initsatus => {} ); 
        //LoadLoadInterstitialAd();
        
    }

  /// <summary>
  /// Loads the interstitial ad.
  /// </summary>
  public void LoadLoadInterstitialAd()
  {
      // Clean up the old ad before loading a new one.
      if (_interstitialAd != null)
      {
            _interstitialAd.Destroy();
            _interstitialAd = null;
      }

      Debug.Log("Loading the interstitial ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();

      // send the request to load the ad.
      InterstitialAd.Load(_adUnitId, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              _interstitialAd = ad;
          });
  }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

  void Update() {
    if (runAd) {
        LoadLoadInterstitialAd();
        runAd = false;
    }

    ShowInterstitialAd();

    
  }
    
}
