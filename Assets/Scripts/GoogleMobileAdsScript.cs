using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private BannerView bannerView;

    private readonly string adBannerUnitId = "ca-app-pub-6488202776624573/5150649824";
    public void Start()
    {
        this.RequestBanner();
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
        AdRequest request = new AdRequest.Builder()
            .AddKeyword("game")
            .AddKeyword("tetris")
            .Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
        bannerView.Show();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
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
}
