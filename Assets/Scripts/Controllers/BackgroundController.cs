using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    ParticleSystem pSys;

    // Start is called before the first frame update
    void Start()
    {
        pSys = GetComponent<ParticleSystem>();
        var main = pSys.main;
        var originalLifetime = main.startLifetime;
        ParticleSystem.MinMaxCurve burstLifetime = new ParticleSystem.MinMaxCurve(0.0f, 30.0f);
        main.startLifetime = burstLifetime;
        pSys.Emit(300);
        main.startLifetime = originalLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
