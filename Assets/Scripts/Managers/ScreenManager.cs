using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Dz = deadzone
//Px = pixels
//U = units

//Rectangles in pixels have a center (0,0) at the bottom left of the screen.
//From Unity Docs - "[Maximum Safe Area] is defined as @@Rect(0, 0, Screen.width, Screen.height)@@"

//Rectangles in units have a center at the center of the screen.
//This choice is made to align with the game coordinates.

// All UI modifications much be made with respect to the reference resolution
// As opposed to the actual screen resolution.

public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField] Image bottomDz;
    [SerializeField] Image topDz;
    
    private Vector2 referenceScale = new Vector2(1080, 1920);
    private float targetPlayHeightU = 15f;
    private float targetPlayArea = 15f * 12f; //Units Squared

    private Rect screenRectPx;
    private Rect safeAreaPx;
    private Rect screenRectU;
    private Rect playRectU;
    private float joystickDzPx_Ref;

    public Rect PlayRectU { get { return playRectU; } }

    private void Start()
    {
        //This is the joystick deadzone height prior to scaling.
        joystickDzPx_Ref = bottomDz.rectTransform.rect.height;
        UpdateScreen();
    }

    private void Update()
    {
        if (Screen.width != screenRectPx.width
            || Screen.height != screenRectPx.height
            || Screen.safeArea != safeAreaPx)
        {
            UpdateScreen();
        }
    }

    private void UpdateScreen()
    {
        screenRectPx = new Rect(0f, 0f, Screen.width, Screen.height);
        safeAreaPx = Screen.safeArea;

        float topDzPx = GetTopDzPx(screenRectPx, safeAreaPx);
        topDz.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, convertToReference(topDzPx));

        float bottomDzPx = GetBottomDzPx(screenRectPx, safeAreaPx);
        bottomDz.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, convertToReference(bottomDzPx));

        //Set camera size so the play area contains a target heigh of units.
        float playHeightPx = screenRectPx.height - topDzPx - bottomDzPx;
        float playPixelArea = playHeightPx * screenRectPx.width;
        float pxPerU = Mathf.Pow(playPixelArea, 0.5f) / Mathf.Pow(targetPlayArea, 0.5f);

        float screenHeightU = Screen.height / pxPerU;
        //float screenHeightU = GetTargetScreenHeightU(topDzPx + bottomDzPx);
        Camera.main.orthographicSize = screenHeightU / 2f;
        
        //float pxPerU = Screen.height / screenHeightU;

        screenRectU = new Rect(0f, 0f, Screen.width / pxPerU, Screen.height / pxPerU);
        screenRectU.center = Vector2.zero;

        //Set Play Area to match
        playRectU = screenRectU;
        playRectU.yMax -= topDzPx / pxPerU;
        playRectU.yMin += bottomDzPx / pxPerU;

        LogRects();
        DevManager.Instance.Set(9, $"screenHeightU: {screenHeightU}");
        DevManager.Instance.Set(18, "Play Area Units: " + playRectU.width * playRectU.height);
    }

    private float GetTopDzPx(Rect screenRectPx, Rect safeAreaPx)
    {
        float safeAreaTopPx = screenRectPx.yMax - safeAreaPx.yMax;
        float adHeightPx = GoogleAdManager.getBannerHeight();
        if (Mathf.Approximately(adHeightPx, 0f))
        {
            adHeightPx = screenRectPx.width * 0.15f;
        }
        return safeAreaTopPx + adHeightPx;
    }

    private float GetBottomDzPx(Rect screenRectPx, Rect safeAreaPx)
    {
        float safeAreaBottomPx = safeAreaPx.yMin - screenRectPx.yMin;
        return convertFromReference(joystickDzPx_Ref) + safeAreaBottomPx;
    }

    private float GetTargetScreenHeightU(float totalDeadzoneHeightPx)
    {
        float playHeight = screenRectPx.height - totalDeadzoneHeightPx;
        float playPixelArea = playHeight * screenRectPx.width;
        float pxPerU = Mathf.Pow(playPixelArea, 0.5f) / Mathf.Pow(targetPlayArea, 0.5f);
        return targetPlayHeightU * screenRectPx.height / playHeight;
    }

    private float convertToReference(float num)
    {
        float ratio = referenceScale.y / screenRectPx.height;
        return num * ratio;
    }

    private float convertFromReference(float num)
    {
        float ratio = screenRectPx.height / referenceScale.y;
        return num * ratio;
    }

    //--------------    SCREEN RELATED UTILS     --------------------

    public static Rect Shrink(Rect rect, float amount)
    {
        rect.xMin += amount;
        rect.xMax -= amount;
        rect.yMin += amount;
        rect.yMax -= amount;
        return rect;
    }


    //--------------------     DEBUGGING     ------------------------

    private void LogRects()
    {
        LogPx(10, "screenRectPx", screenRectPx);
        LogPx(12, "safeAreaPx", safeAreaPx);
        LogU(14, "screenRectU", screenRectU);
        LogU(16, "playRectU", playRectU);
    }

    private void OnDrawGizmos()
    {
        Vector3 center = playRectU.center;
        Vector3 size = new Vector3(playRectU.width, playRectU.height, 0);
        Gizmos.DrawWireCube(center, size);
    }

    private void LogPx(int line, string name, Rect rect)
    {
        DevManager.Instance.Set(line, name);
        DevManager.Instance.Set(line + 1, $"xMax: {rect.xMax:F0} xMin: {rect.xMin:F0} yMax: {rect.yMax:F0} yMin: {rect.yMin:F0}");
    }

    private void LogU(int line, string name, Rect rect)
    {
        DevManager.Instance.Set(line, name);
        DevManager.Instance.Set(line + 1, $"xMax: {rect.xMax:F1} xMin: {rect.xMin:F1} yMax: {rect.yMax:F1} yMin: {rect.yMin:F1}");
    }
}
