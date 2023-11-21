using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public class Sound
    {
        [Tooltip("The name of the sound")]
        public string audioName;

        [Tooltip("The audio clip of the sound")]
        public AudioClip clip;

        [Tooltip("The type of the sound")]
        public SoundType type;

        [Tooltip("The name of the sound")]
        public bool is3D;
    }

    public enum SoundType
    {
        Music,
        SoundEffect,
        BGM,
    }

    public List<Sound> sounds;
    private Dictionary<string, AudioSource> audioSources;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitialiseAudioSources();
            LoadPlayerPrefs();
            AudioManager.instance.PlayAudios("Background Music", transform.position);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitialiseAudioSources()
    {
        audioSources = new Dictionary<string, AudioSource>();

        foreach (Sound sound in sounds)
        {
            GameObject soundObject = new GameObject($"AudioSource_{sound.audioName}");
            soundObject.transform.SetParent(transform);

            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.playOnAwake = false;
            source.spatialBlend = sound.is3D ? 1 : 0;

            audioSources.Add(sound.audioName, source);
        }
    }

    private void LoadPlayerPrefs()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.75f));
        SetSoundEffectVolume(PlayerPrefs.GetFloat("SoundEffects", 075f));
        SetBGMVolume(PlayerPrefs.GetFloat("UISoundsVolume", 0.75f));
    }

    public void PlayAudios(string soundName, Vector3 pos = default)
    {
        if (audioSources.ContainsKey(soundName))
        {
            AudioSource source = audioSources[soundName];
            source.transform.position = pos == default ? transform.position : pos;
            source.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        SetVolume(SoundType.Music, volume);
    }

    public void SetSoundEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
        SetVolume(SoundType.SoundEffect, volume);
    }

    public void SetBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat("BGMVolume", volume);
        SetVolume(SoundType.SoundEffect, volume);
    }

    public void PlayAudioArray(string[] audioNames, Vector3 pos = default)
    {
        if (audioNames.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, audioNames.Length);
            string randomAudioName = audioNames[randomIndex];
            PlayAudios(randomAudioName, pos);
        }
    }

    private void SetVolume(SoundType type, float volume)
    {
        foreach (KeyValuePair<string, AudioSource> entry in audioSources)
        {
            if (sounds.Find(sound => sound.audioName == entry.Key).type == type)
            {
                entry.Value.volume = volume;
            }
        }
    }
}
