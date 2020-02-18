using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Sfx[] sfxs;

    protected override void Awake()
    {
        base.Awake();
        foreach(Sfx sfx in sfxs)
        {
            sfx.source = gameObject.AddComponent<AudioSource>();
            sfx.source.clip = sfx.clip;
            sfx.source.volume = sfx.volume;
        }
    }

    public void PlaySfx(SfxID id)
    {
        Sfx sfx = Array.Find(sfxs, currentSfx => currentSfx.id == id);
        if (sfx == null)
        {
            Debug.LogError($"Sound {sfx.id.ToString()} not found");
            return;
        }
        sfx.source.Play();
    }
}
