using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Sfx[] sfxs;
    [SerializeField] private Song song;
    private Sfx shipSound;

    [SerializeField] private Slider volumeSliderSfx;
    [SerializeField] private Slider volumeSliderSong;

    protected override void Awake()
    {
        base.Awake();
        foreach(Sfx sfx in sfxs)
        {
            sfx.source = gameObject.AddComponent<AudioSource>();
            sfx.source.clip = sfx.clip;
            sfx.source.volume = sfx.volume;
        }
        song.source = gameObject.AddComponent<AudioSource>();
        song.source.clip = song.clip;
    }

    private void Start()
    {
        volumeSliderSfx.value = GameManager.Instance.activeSave.volumeSfx;
        volumeSliderSong.value = GameManager.Instance.activeSave.volumeSfx;
        SetVolumeSfx(GameManager.Instance.activeSave.volumeSfx);
        SetVolumeSong(GameManager.Instance.activeSave.volumeSfx);
        song.source.Play();
        song.source.loop = true;

        shipSound = PlaySfx(SfxID.SHIP_SOUND);
        shipSound.source.volume = 0f;
        shipSound.source.loop = true;

        EventManager.ExitState += ExitState;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.ExitState -= ExitState;
    }

    private void ExitState(GameState state)
    {
        if (state == GameState.PLAY)
        {
            shipSound.source.volume = 0;
        }
    }

    public Sfx PlaySfx(SfxID id)
    {
        Sfx sfx = Array.Find(sfxs, currentSfx => currentSfx.id == id);
        if (sfx == null)
        {
            Debug.LogError($"Sound {sfx.id.ToString()} not found");
            return null;
        }
        sfx.source.Play();
        return sfx;
    }

    public void SetVolumeSfx(float volume)
    {
        GameManager.Instance.activeSave.volumeSfx = volume;
        foreach(Sfx sfx in sfxs)
        {
            sfx.source.volume = sfx.volume * volume;
        }
        Debug.Log("Sfx Volume: " + volume);
    }

    public void SetVolumeShipSound(float volume)
    {
        if (GameManager.Instance.currentState == GameState.PLAY)
        {
            shipSound.source.volume = shipSound.volume * GameManager.Instance.activeSave.volumeSfx * volume;
        }
        else
        {
            shipSound.source.volume = 0f;
        }
    }

    public void SetVolumeSong(float volume)
    {
        GameManager.Instance.activeSave.volumeSfx = volume;
        song.source.volume = volume;
    }

    public void RestartSong()
    {
        Debug.Log("Song time: " + song.source.time);
        Debug.Log("Seconds Per Cycle: " + song.SecondsPerCycle);

        if (song.source.time > song.SecondsPerCycle)
        {
            float currentCycleTime = song.source.time % song.SecondsPerCycle;
            Debug.Log("Current Cycle Time: " + currentCycleTime);
            song.source.time = currentCycleTime;
        }
    }
}
