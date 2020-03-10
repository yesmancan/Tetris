using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsRequests : MonoBehaviour
{
    public static GoogleMobileAdsScript admob;

    public bool Interstitial;
    public bool RewardBasedVideo;

    public bool Banner;
    public AdPosition BannerPostion;

    void Start()
    {
        admob = GetComponent<GoogleMobileAdsScript>();
        StartCoroutine(RequestAds());
    }

    public IEnumerator RequestAds()
    {
        yield return new WaitForSeconds(1f);
        if (RewardBasedVideo)
        {
            admob.RequestRewardBasedVideo();
        }
        if (Interstitial)
        {
            admob.RequestInterstitial();
        }
        if (Banner)
        {
            admob.RequestBanner(BannerPostion);
        }
    }
}