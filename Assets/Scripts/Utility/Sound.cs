using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SfxID
{
    EXPLOSION_LARGE,
    EXPLOSION_SMALL,
    ENEMY_SPAWN,
    PLAYER_DIED,
    SHIP_SOUND,
}

public enum SongID
{
    THEME,
}

public class Sound
{
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public class Song : Sound
{
    public SongID id;
    public float bpm;
    public float secondsToFirstCycle;
    public float beatsPerCycle;

    public float SecondsPerCycle { get { return 60f / bpm * beatsPerCycle; } }
}

[System.Serializable]
public class Sfx : Sound
{
    public SfxID id;

    [Range(0f, 1f)]
    public float volume;
}
