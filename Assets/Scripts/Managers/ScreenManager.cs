using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Dz = deadzone
//Px = pixels
//U = units

//Rectangles in units have a center at the center of the screen.
//This choice is made to align with the game coordinates.

// All UI modifications much be made with respect to the reference resolution
// As opposed to the actual screen resolution.

// To accurately modify the ui, 3 unit systems need to be taken into account.
// 1) Pixels: The pixels that make up the current screen.
// 2) Units: The units used in the game.
// 3) refPixels: The pixels that make up the reference screen size in the canvas scaler.

public enum ScreenUnits { pixels, units, refPixels }

public struct MultiUnitRect
{
    public Rect pixels;
    public Rect units;
    public Rect refPixels;

    public MultiUnitRect(Rect init, ScreenUnits initUnits, float pxPerU, float refRatio)
    {
        pixels = Rect.zero;
        units = Rect.zero;
        refPixels = Rect.zero;

        switch (initUnits)
        {
            case ScreenUnits.pixels:
                pixels = init;
                units = ScaleRect(pixels, 1 / pxPerU);
                refPixels = ScaleRect(pixels, refRatio);
                break;
            case ScreenUnits.units:
                units = init;
                pixels = ScaleRect(units, pxPerU);
                refPixels = ScaleRect(pixels, 1 / pxPerU);
                break;
            case ScreenUnits.refPixels:
                refPixels = init;
                pixels = ScaleRect(refPixels, 1 / refRatio);
                units = ScaleRect(pixels, 1 / pxPerU);
                break;
        }
    }

    private static Rect ScaleRect(Rect rect, float factor)
    {
        return new Rect(
            rect.x * factor,
            rect.y * factor,
            rect.width * factor,
            rect.height * factor
        );
    }
}

public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField] Image bottomDz;
    [SerializeField] Image topDz;
    
    private Vector2 referenceScale = new Vector2(1080, 1920);
    private float targetPlayAreaUnits = 15f * 12f; //Units Squared

    //private Rect screenRectPx;
    //private Rect safeRectPx;
    //private Rect playRectPx;
    //private Rect screenRectU;
    //private Rect playRectU;
    //private float joystickDzPx_Ref;

    private MultiUnitRect screenArea;
    private MultiUnitRect safeArea;
    private MultiUnitRect playArea;
    private MultiUnitRect joystickDz;
    private Rect joystickDzHeightRefPx;

    public Rect PlayRectU { get { return playArea.units; } }

    private void Start()
    {
        //This is the joystick deadzone prior to scaling.
        joystickDzHeightRefPx = bottomDz.rectTransform.rect;

        UpdateScreen();
    }

    private void Update()
    {
        if (Screen.width != screenArea.pixels.width
            || Screen.height != screenArea.pixels.height
            || Screen.safeArea != safeArea.pixels)
        {
            UpdateScreen();
        }
    }

    private void UpdateScreen()
    {
        //From Unity Docs - "[Maximum Safe Area] is defined as @@Rect(0, 0, Screen.width, Screen.height)@@"
        //So the screen area and safe area are represented with the bottom left corner at 0,0
        Rect screenRectPx = new Rect(0f, 0f, Screen.width, Screen.height);
        Rect safeRectPx = Screen.safeArea;

        float refRatio = referenceScale.y / screenRectPx.height;

        float topDzPx = GetTopDzPx(screenRectPx, safeRectPx);
        float bottomDzPx = GetBottomDzPx(screenRectPx, safeRectPx, refRatio);

        Rect playRectPx = screenRectPx;
        //I Decided to move the play rect's center to (0,0) to align with game coordinates
        playRectPx.center = Vector2.zero;
        playRectPx.yMax -= topDzPx;
        playRectPx.yMin += bottomDzPx;

        float pxPerU = GetPxPerU(playRectPx);

        screenArea = new MultiUnitRect(screenRectPx, ScreenUnits.pixels, pxPerU, refRatio);
        safeArea = new MultiUnitRect(safeRectPx, ScreenUnits.pixels, pxPerU, refRatio);
        playArea = new MultiUnitRect(playRectPx, ScreenUnits.pixels, pxPerU, refRatio);

        //Update Camera
        Camera.main.orthographicSize = screenArea.units.height / 2f;

        //Update UI
        topDz.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, topDzPx * refRatio);
        bottomDz.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bottomDzPx * refRatio);

        Log(5);
    }

    private float GetPxPerU(Rect playRectPx)
    {
        float playAreaPixels = playRectPx.height * playRectPx.width;

        return Mathf.Pow(playAreaPixels, 0.5f) / Mathf.Pow(targetPlayAreaUnits, 0.5f);
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

    private float GetBottomDzPx(Rect screenRectPx, Rect safeAreaPx, float refRatio)
    {
        float safeAreaBottomPx = safeAreaPx.yMin - screenRectPx.yMin;
        return joystickDzHeightRefPx.height / refRatio + safeAreaBottomPx;
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

    private void Log(int startLine)
    {
        LogPx(startLine, "Screen Area Pixels", screenArea.pixels);
        LogPx(startLine + 2, "Safe Area Pixels", safeArea.pixels);
        LogU(startLine + 4, "Screen Area Units", screenArea.units);
        LogU(startLine + 6, "Play Area Units", playArea.units);
        DevManager.Instance.Set(startLine + 8, "Play Area (Units^2): " + playArea.units.width * playArea.units.height);
    }

    private void OnDrawGizmos()
    {
        Vector3 center = playArea.units.center;
        Vector3 size = new Vector3(playArea.units.width, playArea.units.height, 0);
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
