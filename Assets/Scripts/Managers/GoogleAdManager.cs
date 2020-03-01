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


    [SerializeField] private string testAndroidAppId = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] private string testIphoneAppId = "ca-app-pub-3940256099942544/2934735716";

    [SerializeField] private bool useProductionAds = false;
    [SerializeField] private string productionAndroidAppId = "ca-app-pub-9013467281341157/6292838546";
    [SerializeField] private string productionIphoneAppId = "ca-app-pub-9013467281341157/4625656788";

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

#if UNITY_ANDROID
        string adUnitId = useProductionAds ? productionAndroidAppId : testAndroidAppId;
#elif UNITY_IPHONE
        string adUnitId = useProductionAds ? productionIphoneAppId : testIphoneAppId;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, getBannerSize(), AdPosition.Top);

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

    public static AdSize getBannerSize()
    {
        AdSize bannerSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        Debug.Log("FullWidth: " + AdSize.FullWidth);
        Debug.Log("Banner Width: " + bannerSize.Width);
        Debug.Log("Banner Height: " + bannerSize.Height);
        Debug.Log("DEVICE SCALE: " + MobileAds.Utils.GetDeviceScale());
        DevManager.Instance.Set(22, "Banner Width: " + bannerSize.Width);
        DevManager.Instance.Set(23, "Banner Height: " + bannerSize.Height);
        DevManager.Instance.Set(24, "Device Scale: " + MobileAds.Utils.GetDeviceScale());
        return bannerSize;
    }

    public static int getBannerHeight()
    {
        return getBannerSize().Height;
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
