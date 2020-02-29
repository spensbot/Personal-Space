using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField] Image joystickDeadzone;
    [SerializeField] Image googleAdDeadzone;

    private Rect screenRectUnits;
    private Rect screenRectPixels;
    private Rect playRectUnits;
    private Rect safeAreaPixels = new Rect(0, 0, 0, 0);
    private float pixelsPerUnit;

    public Rect PlayRectUnits { get { return playRectUnits; } }

    private void Start()
    {
        UpdateScreenDims();
        UpdateSafeArea(Screen.safeArea);
    }

    private void Update()
    {
        if (Screen.safeArea != safeAreaPixels)
        {
            UpdateScreenDims();
            UpdateSafeArea(Screen.safeArea);
        }
    }

    private void UpdateSafeArea(Rect newSafeArea)
    {
        safeAreaPixels = newSafeArea;

        //Adjust ad deadzone
        //int adHeightPixels = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth).Height;
        float safeAreaTopHeightPixels = screenRectPixels.yMax - safeAreaPixels.yMax;
        float adHeightPixels = safeAreaPixels.width * 0.15f;
        float totalTopDeadzoneHeightPixels = safeAreaTopHeightPixels + adHeightPixels;
        googleAdDeadzone.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalTopDeadzoneHeightPixels);

        //Adjust joystick deadzone
        float joystickDeadzoneHeightPixels = 4f / 20f * safeAreaPixels.height;
        float safeAreaBottomHeightPixels = safeAreaPixels.yMin - screenRectPixels.yMin;
        float totalBottomDeadzoneHeightPixels = joystickDeadzoneHeightPixels + safeAreaBottomHeightPixels;
        joystickDeadzone.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalBottomDeadzoneHeightPixels);

        //Set Play Area to match
        playRectUnits = screenRectUnits;
        playRectUnits.yMax -= totalTopDeadzoneHeightPixels / pixelsPerUnit;
        playRectUnits.yMax += totalBottomDeadzoneHeightPixels / pixelsPerUnit;
        playRectUnits.center = Vector2.zero;

        DebugLog();
    }

    private void UpdateScreenDims()
    {
        float screenHeightUnits = Camera.main.orthographicSize * 2f;
        pixelsPerUnit = Screen.height / screenHeightUnits;

        screenRectPixels = new Rect(0f, 0f, Screen.width, Screen.height);
        screenRectUnits = new Rect(0f, 0f, Screen.width/pixelsPerUnit, Screen.height/pixelsPerUnit);
    }

    public static Rect Shrink(Rect rect, float amount)
    {
        rect.xMin += amount;
        rect.xMax -= amount;
        rect.yMin += amount;
        rect.yMax -= amount;
        return rect;
    }

    private void OnDrawGizmos()
    {
        Vector3 center = playRectUnits.center;
        Vector3 size = new Vector3(playRectUnits.width, playRectUnits.height, 0);
        Gizmos.DrawWireCube(center, size);
    }

    private void DebugLog()
    {
        DevManager.Instance.Set(10, $"Screen width: {Screen.width} height: {Screen.height}");
        DevManager.Instance.Set(11, $"Safe Area yMax: {Screen.safeArea.yMax} yMin: {Screen.safeArea.yMin}");
        DevManager.Instance.Set(12, $"Screen yMax: {screenRectPixels.yMax} yMin: {screenRectPixels.yMin}");
        DevManager.Instance.Set(13, $"Play Area xMax: {playRectUnits.xMax} xMin: {playRectUnits.xMin}");
        DevManager.Instance.Set(14, $"Play Area yMax: {playRectUnits.yMax} yMin: {playRectUnits.yMin}");
        DevManager.Instance.Set(15, $"Screen Area xMax: {screenRectUnits.xMax} xMin: {screenRectUnits.xMin}");
        DevManager.Instance.Set(16, $"Screen Area yMax: {screenRectUnits.yMax} yMin: {screenRectUnits.yMin}");
    }
}
