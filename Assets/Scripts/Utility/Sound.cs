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
}

public enum SongID
{
    MAIN,
}

public class Sound
{
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public class Song : Sound
{
    public SongID id;
    public float bpm;
    public float secondsToFirstChord;
    public float beatsPerProgression;

}

[System.Serializable]
public class Sfx : Sound
{
    public SfxID id;
}
