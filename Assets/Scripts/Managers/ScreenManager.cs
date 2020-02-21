using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField] Image joystickDeadzone;
    [SerializeField] Image googleAdDeadzone;

    public Rect screenRectPixels { get; private set; }
    public Rect screenRect { get; private set; }
    public Rect playRect;
    public Rect bombSpawnRect;
    public float pixelsPerUnit { get; private set; }
    public float screenHeight { get; private set; } //Unity Units that make up the screen height.
    public float googleAdHeight { get; private set; }
    public float joystickHeight { get; private set; }


    private void Start()
    {
        screenHeight = Camera.main.orthographicSize * 2f;
        pixelsPerUnit = Screen.height / screenHeight;

        float widthPixels = (int)Screen.width;
        float heightPixels = (int)Screen.height;
        float width = widthPixels / pixelsPerUnit;
        float height = heightPixels / pixelsPerUnit;
        screenRectPixels = new Rect(-widthPixels / 2, -heightPixels / 2, widthPixels, heightPixels);
        screenRect = new Rect(-width / 2, -height / 2, width, height);

        AdSize adaptiveBannerSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        float googleAdHeightPixels = adaptiveBannerSize.Height;
        googleAdHeight = googleAdHeightPixels / pixelsPerUnit;
        float deadzoneWidth = googleAdDeadzone.rectTransform.rect.width;
        Debug.Log("DeadzoneWidth: " + deadzoneWidth);
        googleAdDeadzone.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, deadzoneWidth * 0.15f);

        joystickHeight = joystickDeadzone.minHeight / pixelsPerUnit;

        playRect = screenRect;
        playRect.yMin += 4;
        playRect.yMax -= screenRect.width * 0.15f;

        bombSpawnRect = shrink(playRect, 1f);

        DevManager.Instance.Set(10, $"Screen width: {Screen.width} height: {Screen.height}");
        DevManager.Instance.Set(11, $"Safe Area width: {Screen.safeArea.width} height: {Screen.safeArea.height}");
        DevManager.Instance.Set(12, $"Units width: {width} height: {height}");
        DevManager.Instance.Set(13, $"DPI: {Screen.dpi}");
        DevManager.Instance.Set(14, $"screenRect yMin: {screenRect.yMin} yMax: {screenRect.yMax}");
        DevManager.Instance.Set(15, $"PlayRect yMin: {playRect.yMin} yMax: {playRect.yMax}");
    }

    private Rect shrink(Rect rect, float amount)
    {
        rect.xMin += amount;
        rect.xMax -= amount;
        rect.yMin += amount;
        rect.yMax -= amount;
        return rect;
    }

    private void OnDrawGizmos()
    {
        Vector3 center = playRect.center;
        Vector3 size = new Vector3(playRect.width, playRect.height, 0);
        Gizmos.DrawWireCube(center, size);
    }
}
