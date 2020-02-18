using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GoogleAdManager : Singleton<GoogleAdManager>
{
    private BannerView bannerView;
    private int attempts;
    public int adWidthScaled { get; private set; }
    public int adHeightScaled { get; private set; }
    public int adWidthUnscaled { get; private set; }
    public int adHeightUnscaled { get; private set; }

    public void Start()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-9013467281341157~2744475776";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
        #else
            string appId = "unexpected_platform";
        #endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
    }

    private void RequestBanner()
    {
        attempts += 1;

        //Test adUnitIds from Google.
        //Real android bannerId: ca-app-pub-9013467281341157/6292838546
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        AdSize adaptiveBannerSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        Debug.Log("Screen.widht: " + Screen.width);
        Debug.Log("Adsize.Fullwidth: " + AdSize.FullWidth);
        Debug.Log("Google Device Scale: " + MobileAds.Utils.GetDeviceScale());

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, adaptiveBannerSize, AdPosition.Top);

        //---------     AD LIFECYCLE HOOKS     ------------------

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

        DevManager.Instance.Set(20, $"Banner Requested {this.attempts} times");
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        DevManager.Instance.Set(20, $"BannerView width: {this.bannerView.GetWidthInPixels()}");
        DevManager.Instance.Set(21, $"BannerView height: {this.bannerView.GetHeightInPixels()}");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DevManager.Instance.Set(21, "HandleFailedToReceiveAd event received with message: " + args.Message);
        Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
        Invoke("RequestBanner", 5.0f);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeavingApplication event received");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        bannerView.Destroy();
    }
}
