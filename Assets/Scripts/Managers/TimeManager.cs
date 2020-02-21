using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager: Singleton<TimeManager>
{
    //Unitys physics engine updates every 0.02 seconds given a timeScale of 1.0.
    //We need to update this value when we slow down the game to prevent jitter.
    private readonly float dtFixed = 0.02f;

    public void SetTimeScale(float timeScale)
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
                "onupdate", "SetTimeScale",
                "easetype", iTween.EaseType.easeOutQuad
            )
        );
    }

    public void ClearEffects()
    {
        iTween.Stop(gameObject);
    }
}
