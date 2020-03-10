using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            //.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            .AddKeyword("game")
            //.SetGender(Gender.Male)
            //.SetBirthday(new DateTime(1998, 02, 23))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }


    [Header("Android")]
    public VGAdmobID AndroidID = new VGAdmobID()
    {
        AppID = "ca-app-pub-6488202776624573~9857903798",
        BannerID = "ca-app-pub-6488202776624573/5150649824",
        RewardID = "",
        InterstitialID = "ca-app-pub-6488202776624573/3261841325",

    };
    [Header("IOS")]
    public VGAdmobID IOSID = new VGAdmobID()
    {
        AppID = "",
        BannerID = "",
        RewardID = "",
        InterstitialID = "",

    };

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;
    private static string outputMessage = string.Empty;
    public static string OutputMessage
    {
        set { outputMessage = value; }
    }
    public void Start()
    {
#if UNITY_ANDROID
        string appId = AndroidID.AppID;
#elif UNITY_IPHONE
        string appId = IOSID.AppID;
#else
        string appId = "unexpected_platform";
#endif

        MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // RewardBasedVideoAd is a singleton, so rs should only be registered once.
        this.rewardBasedVideo.OnAdLoaded += this.RewardBasedVideoLoaded;
        this.rewardBasedVideo.OnAdFailedToLoad += this.RewardBasedVideoFailedToLoad;
        this.rewardBasedVideo.OnAdOpening += this.RewardBasedVideoOpened;
        this.rewardBasedVideo.OnAdStarted += this.RewardBasedVideoStarted;
        this.rewardBasedVideo.OnAdRewarded += this.RewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.RewardBasedVideoClosed;
        this.rewardBasedVideo.OnAdLeavingApplication += this.RewardBasedVideoLeftApplication;

        RequestBanner(AdPosition.Bottom);
        RequestInterstitial();
    }
    public void DestroyBanner()
    {
        this.bannerView.Destroy();
    }
    public void RequestBanner(AdPosition poz)
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = AndroidID.BannerID;
#elif UNITY_ANDROID
        string adUnitId = AndroidID.BannerID;
#elif UNITY_IPHONE
        string adUnitId = IOSID.BannerID;
#else
        string adUnitId = "unexpected_platform";
#endif


        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, poz);
        // Register for ad events.
        this.bannerView.OnAdLoaded += this.AdLoaded;
        this.bannerView.OnAdFailedToLoad += this.AdFailedToLoad;
        this.bannerView.OnAdOpening += this.AdOpened;
        this.bannerView.OnAdClosed += this.AdClosed;
        this.bannerView.OnAdLeavingApplication += this.AdLeftApplication;

        // Load a banner ad.
        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    public void RequestInterstitial()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = AndroidID.InterstitialID;
#elif UNITY_ANDROID
        string adUnitId = AndroidID.InterstitialID;
#elif UNITY_IPHONE
        // string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        string adUnitId = IOSID.InterstitialID;
#else
        string adUnitId = "unexpected_platform";
#endif


        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.InterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.InterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.InterstitialOpened;
        this.interstitial.OnAdClosed += this.InterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.InterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }
    public void RequestRewardBasedVideo()
    {
#if UNITY_EDITOR
        string adUnitId = AndroidID.RewardID;
#elif UNITY_ANDROID
        string adUnitId = AndroidID.RewardID;
#elif UNITY_IPHONE
        // string adUnitId = "ca-app-pub-3940256099942544/1712485313";
        string adUnitId = IOSID.RewardID;
#else
        string adUnitId = "unexpected_platform";
#endif
        this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
    }
    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            Debug.Log("Interstitial is not ready yet");
        }
    }

    public void ShowRewardBasedVideo()
    {
        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            RequestRewardBasedVideo();
            Debug.Log("Reward based video ad is not ready yet");
        }
    }

    public IEnumerator RequestAds(string type)
    {
        yield return new WaitForSeconds(1f);
        if (type == "RewardBasedVideo")
        {
            RequestRewardBasedVideo();
        }
        if (type == "Interstitial")
        {
            RequestInterstitial();
        }
    }

    #region Banner callback rs

    public void AdLoaded(object sender, EventArgs args)
    {
        Debug.Log("AdLoaded event received");
    }

    public void AdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("FailedToReceiveAd event received with message: " + args.Message);
    }

    public void AdOpened(object sender, EventArgs args)
    {
        Debug.Log("AdOpened event received");
    }

    public void AdClosed(object sender, EventArgs args)
    {
        Debug.Log("AdClosed event received");
    }

    public void AdLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("AdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback rs

    public void InterstitialLoaded(object sender, EventArgs args)
    {
        Debug.Log("InterstitialLoaded event received");
        ShowInterstitial();
    }

    public void InterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(
            "InterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void InterstitialOpened(object sender, EventArgs args)
    {
        Debug.Log("InterstitialOpened event received");
    }

    public void InterstitialClosed(object sender, EventArgs args)
    {
        Debug.Log("InterstitialClosed event received");
    }

    public void InterstitialLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("InterstitialLeftApplication event received");
    }

    #endregion

    #region RewardBasedVideo callback rs

    public void RewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("RewardBasedVideoLoaded event received");
    }

    public void RewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(
            "RewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void RewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("RewardBasedVideoOpened event received");
    }

    public void RewardBasedVideoStarted(object sender, EventArgs args)
    {
        Debug.Log("RewardBasedVideoStarted event received");
    }

    public void RewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("RewardBasedVideoClosed event received");
        // Yazi.text = "Video Kapandi";
        RequestRewardBasedVideo();
    }

    public void RewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log(
            "RewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
        RequestRewardBasedVideo();
    }

    public void RewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("RewardBasedVideoLeftApplication event received");
    }

    #endregion
}
[System.Serializable]
public class VGAdmobID
{
    public string AppID;
    public string BannerID;
    public string RewardID;
    public string InterstitialID;
}
