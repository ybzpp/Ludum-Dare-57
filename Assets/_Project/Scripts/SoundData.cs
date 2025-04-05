using System;
using UnityEngine;

[Serializable]
public class SoundGroup
{
    public string groupName;
    public AudioClip[] clips;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 1f)] public float probability = 1f; // Вероятность проигрывания звука (для прыжков 0.7)
    public bool mute = false;
}

[Serializable]
public class BackgroundSound
{
    public string name;
    public AudioClip[] clips;
    [Range(0f,1f)] public float volume = 1f;
    public bool mute = false;
}

[CreateAssetMenu]
public class SoundData : ScriptableObject
{
    public SoundGroup[] soundGroups;
    public BackgroundSound[] backgroundSounds;
}
