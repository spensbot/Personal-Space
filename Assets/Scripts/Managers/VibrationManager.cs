using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDG;

public class VibrationManager: Singleton<VibrationManager>
{
    public void SetVibration(bool isVibration) {
        Debug.Log("Is Vibration: " + isVibration);
        GameManager.Instance.activeSave.isVibration = isVibration;
    }

    public void DecayingVibrate(long duration, int initIntensity, long stepTime, bool alternate)
    {
        if (GameManager.Instance.activeSave.isVibration)
        {
            if (stepTime < 30)
            {
                Debug.LogWarning("Some phones may not respond to a vibration time of <30ms");
            }

            long numSteps = duration / stepTime;
            long[] pattern = new long[numSteps];
            int[] amplitudes = new int[numSteps];

            for (int i = 0; i < numSteps; i++)
            {
                pattern[i] = stepTime;
                if (i % 2 == 0)
                {
                    amplitudes[i] = initIntensity;
                    initIntensity = initIntensity / 2;
                }
                else if (!alternate)
                {
                    amplitudes[i] = initIntensity;
                    initIntensity = initIntensity / 2;
                }
                else
                {
                    amplitudes[i] = 0;
                }
            }

            Vibration.Vibrate(pattern, amplitudes);

        }
    }
}
