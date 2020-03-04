using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager: Singleton<TimeManager>
{
    //Unitys physics engine updates every 0.02 seconds given a timeScale of 1.0.
    //We need to update this value when we slow down the game to prevent jitter.
    private readonly float dtFixed = 0.02f;

    [HideInInspector]
    public float mainTimeScale = 0f;
    [HideInInspector]
    public float debugTimeScale = 1f;
    [HideInInspector]
    public float effectTimeScale = 1f;

    private void Start()
    {
        effectTimeScale = 1f;
        debugTimeScale = 1f;
    }

    private void Update()
    {
        SetTimeScale(mainTimeScale * debugTimeScale * effectTimeScale);
        DevManager.Instance.Set(25, "Main Time Scale: " + mainTimeScale);
        DevManager.Instance.Set(26, "Debug Time Scale: " + debugTimeScale);
        DevManager.Instance.Set(27, "Effect Time Scale: " + effectTimeScale);
    }

    private void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = dtFixed * timeScale;
    }

    public void SlowTimeEffect(float start, float duration)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
                "from", start, //0.3
                "to", 1.0f,
                "time", duration, //0.4
                "onupdatetarget", gameObject,
                "onupdate", "SetEffectTimeScale",
                "easetype", iTween.EaseType.easeOutQuad
            )
        );
    }

    private void SetEffectTimeScale(float newEffectTimeScale)
    {
        effectTimeScale = newEffectTimeScale;
    }

    public void ClearEffects()
    {
        iTween.Stop(gameObject);
    }
}
