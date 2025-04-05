using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public static class AudioHelper
{
    public static void PlaySound(string groupName, Vector3 position = default)
    {
        AudioManager.Instance?.PlaySound(groupName, position);
    }

    public static AudioClip GetClipByIndex(string groupName, int index)
    {
        return AudioManager.Instance.GetClipByIndex(groupName, index);
    }

    public static int GetGroupLength(string groupName)
    {
        return AudioManager.Instance.GetGroupLength(groupName);
    }

    public static void PlaySound(string groupName, AudioSource audioSource)
    {
        AudioManager.Instance?.PlaySound(groupName, audioSource);
    }

    public static void PlayMusic()
    {
        AudioManager.Instance?.StartBackgroundMusic();
    }

    public static void StopMusic()
    {
    }
}

[Serializable]
public class AudioSettings
{
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float effectsVolume = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private SoundData soundConfig;
    [SerializeField] private AudioMixer audioMixer;

    private AudioSource[] audioSources; // Пул аудио источников
    private AudioSource musicSource; // Для фоновой музыки
    private AudioSource ambienceSource; // Для фоновых шумов
    private AudioSettings currentSettings;

    private const int AUDIO_SOURCES_POOL_SIZE = 10;
    private const string SETTINGS_FILE = "audioSettings.json";
    private string SettingsPath => Path.Combine(Application.persistentDataPath, SETTINGS_FILE);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeAudioSources()
    {
        // Создаём пул аудио источников

        if (audioSources == null || audioSources.Length == 0)
        {
            audioSources = new AudioSource[AUDIO_SOURCES_POOL_SIZE];
            for (int i = 0; i < AUDIO_SOURCES_POOL_SIZE; i++)
            {
                var go = new GameObject($"AudioSourcePool_{i}");
                go.transform.parent = transform;
                audioSources[i] = go.AddComponent<AudioSource>();
                audioSources[i].spatialBlend = 1;
            }
        }

        // Отдельные источники для фоновых звуков
        if (!musicSource)
        {
            var musicGO = new GameObject($"AudioMusicSourcePool");
            musicGO.transform.parent = transform;
            musicSource = musicGO.AddComponent<AudioSource>();
        }

        if (!ambienceSource)
        {
            var ambienceGO = new GameObject($"AudioAmbienceSourcePool");
            ambienceGO.transform.parent = transform;
            ambienceSource = ambienceGO.AddComponent<AudioSource>();
        }

        // Настраиваем выходы для микшера
        SetupAudioMixer();
        LoadSettings();
        ApplySettings();
    }

    private void SetupAudioMixer()
    {
        // Подключаем источники к соответствующим группам в микшере
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        ambienceSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];

        foreach (var source in audioSources)
        {
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effects")[0];
        }
    }

    private AudioSource GetFreeAudioSource()
    {
        return audioSources.FirstOrDefault(source => !source.isPlaying) ?? audioSources[0];
    }

    public void PlaySound(string groupName, Vector3 position = default)
    {
        var group = soundConfig.soundGroups.FirstOrDefault(g => g.groupName == groupName);
        if (group == null || group.clips.Length == 0 || group.mute) return;

        // Проверяем вероятность проигрывания
        if (group.probability != 1 && UnityEngine.Random.value > group.probability) return;

        var clip = group.clips[UnityEngine.Random.Range(0, group.clips.Length)];
        var source = GetFreeAudioSource();

        source.spatialBlend = position == default ? 0 : 1;
        source.clip = clip;
        source.volume = group.volume;
        source.loop = false;
        source.transform.position = position;
        source.PlayOneShot(clip);
    }

    public int GetGroupLength(string groupName)
    {
        var group = soundConfig.soundGroups.FirstOrDefault(g => g.groupName == groupName);
        if (group == null || group.clips.Length == 0) return 0;
        return group.clips.Length;
    }

    public AudioClip GetClipByIndex(string groupName, int index)
    {
        var group = soundConfig.soundGroups.FirstOrDefault(g => g.groupName == groupName);

        if (group == null || group.clips.Length == 0 || group.mute) 
            return null;

        return group.clips[index];
    }

    internal void PlaySound(string groupName, AudioSource source)
    {
        Debug.Log($"PlaySound :{groupName}");
        var group = soundConfig.soundGroups.FirstOrDefault(g => g.groupName == groupName);
        if (group == null || group.clips.Length == 0 || group.mute)
            return;

        if (group.probability != 1 && UnityEngine.Random.value > group.probability) 
            return;

        var clip = group.clips[UnityEngine.Random.Range(0, group.clips.Length)];
        Debug.Log($"PlaySound2 :{groupName}");
        source.spatialBlend = 1f;
        source.clip = clip;
        source.volume = group.volume;
        source.loop = false;
        source.PlayOneShot(clip);
    }

    public void StartBackgroundMusic()
    {
        var group = soundConfig.backgroundSounds
            .Where(s => s.name == "Music")
            .ToArray();

        if (group.Length == 0) return;

        PlayBackgroundSound(musicSource, group);
        musicSource.loop = false;

        // Подписываемся на событие окончания трека
        _waitForEndCoroutine = StartCoroutine(WaitForEnd(musicSource));
    }

    private Coroutine _waitForEndCoroutine;

    private System.Collections.IEnumerator WaitForEnd(AudioSource audioSource)
    {
        while (true)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying && audioSource.gameObject.activeInHierarchy);
            StartBackgroundMusic(); // Запускаем следующий трек
        }
    }

    public void StopMusicSound()
    {
        Debug.Log("StopMusicSound");

        if (_waitForEndCoroutine != null)
            StopCoroutine(_waitForEndCoroutine);

        musicSource.Stop();
        ambienceSource.Stop();
    }

    private void PlayBackgroundSound(AudioSource source, BackgroundSound[] sounds)
    {
        if (!source.gameObject.activeInHierarchy)
            return;

        var sound = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        Debug.Log($"sound.clips:{sound.clips}");
        var clip = sound.clips[UnityEngine.Random.Range(0, sound.clips.Length)];

        if (source.mute)
            return;

        source.clip = clip;
        source.volume = sound.volume;
        source.Play();
    }

    // Методы для настройки громкости
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Convert(volume));
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("Effects", Convert(volume));
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Convert(volume));  
    }

    private float Convert(float volume)
    {
        var fixVolume = volume == 0 ? -80 : Mathf.Log10(volume) * 20;
        return fixVolume;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                string json = File.ReadAllText(SettingsPath);
                currentSettings = JsonUtility.FromJson<AudioSettings>(json);
            }
            else
            {
                currentSettings = new AudioSettings();
            }

            // Применяем загруженные настройки
            ApplySettings();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading audio settings: {e.Message}");
            currentSettings = new AudioSettings();
        }
    }

    private void ApplySettings()
    {
        SetMasterVolume(currentSettings.masterVolume);
        SetMusicVolume(currentSettings.musicVolume);
        SetEffectsVolume(currentSettings.effectsVolume);
    }

    public void SaveSettings()
    {
        try
        {
            string json = JsonUtility.ToJson(currentSettings, true);
            File.WriteAllText(SettingsPath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving audio settings: {e.Message}");
        }
    }

    // Методы обновления настроек
    public void UpdateMasterVolume(float volume)
    {
        currentSettings.masterVolume = volume;
        SaveSettings();
        ApplySettings();
    }

    public void UpdateMenuMusicVolume(float volume)
    {
        currentSettings.musicVolume = volume;
        SaveSettings();
        ApplySettings();
    }

    public void UpdateMenuEffectsVolume(float volume)
    {
        currentSettings.effectsVolume = volume;
        SaveSettings();
        ApplySettings();
    }

    public float GetMasterVolume() => currentSettings.masterVolume;
    public float GetMenuMusicVolume() => currentSettings.musicVolume;
    public float GetMenuEffectsVolume() => currentSettings.effectsVolume;

}