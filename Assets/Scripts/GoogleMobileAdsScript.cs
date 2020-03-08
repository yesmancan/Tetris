using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd fullscreenAd;

    private readonly string appID = "ca-app-pub-6488202776624573~9857903798";
    private readonly string adBannerUnitId = "ca-app-pub-6488202776624573/5150649824";
    private readonly string adInterstitialUnitId = "ca-app-pub-6488202776624573/3261841325f";
    private void Awake()
    {
        MobileAds.Initialize(appID);
    }
    public void Start()
    {
        try
        {
            this.RequestFullScreenAds();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        try
        {
            this.RequestBanner();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    //private AdRequest CreateAdRequest()
    //{
    //    return new AdRequest.Builder()
    //        .AddTestDevice(AdRequest.TestDeviceSimulator)
    //        .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
    //        .AddKeyword("game")
    //        //.SetGender(Gender.Male)
    //        //.SetBirthday(new DateTime(1985, 1, 1))
    //        //.TagForChildDirectedTreatment(false)
    //        .AddExtra("color_bg", "9B30FF")
    //        .Build();
    //}
    private void RequestBanner()
    {

        this.bannerView = new BannerView(adBannerUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
        bannerView.Show();
    }
    private void RequestFullScreenAds()
    {
        if (this.fullscreenAd != null)
        {
            this.fullscreenAd.Destroy();
        }

        fullscreenAd = new InterstitialAd(adInterstitialUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        fullscreenAd.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);

        ExampleCoroutine(10);
        this.RequestBanner();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    IEnumerator ExampleCoroutine(int second)
    {
        yield return new WaitForSeconds(second);
    }
}
