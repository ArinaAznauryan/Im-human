using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;

public class rewardedAd : MonoBehaviour
{
    #if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-7333197465910069/3819375832";
    #elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-7333197465910069/3819375832";/*"ca-app-pub-3940256099942544/1712485313"*/;/*"ca-app-pub-7333197465910069/3819375832";*/
    #else
    private string _adUnitId = "unused";
    #endif

    private RewardedAd _rewardedAd;

    public bool runAd = false, once = true, loadAd = true;

    public UnityEvent additEvent;

    public GameObject eventHandlerObject;

    void Start()
    {
        MobileAds.Initialize(initstatus => {});
        
        LoadRewardedAd();
    }

    public void PlayAd() {
        runAd = true;
    }

    void Update() {


        if (runAd) {
            ShowRewardedAd();
            
            once = true;
        }

        if (loadAd) {
            Debug.Log("STFU, LIL NIGGA");
            LoadRewardedAd();
            loadAd = false;
        }

        if(_rewardedAd != null) RegisterEventHandlers(_rewardedAd);
    }

  /// <summary>
  /// Loads the rewarded ad.
  /// </summary>
    public void LoadRewardedAd()
    {
         
        //UnityEngine.EventSystems.EventSystem.current.enabled = false;
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
        Debug.Log("AU REWARDER AD AUR: " + _rewardedAd);
        
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

       

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

            // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                    "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");

            if (once) {
                additEvent.Invoke();
                once = false;
                runAd = false;
                loadAd = true;
            }
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                        "with error : " + error);
        };
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing REWARDED ad.");
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format("Ad rewarded!: ", reward.Type, reward.Amount));
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }
}
