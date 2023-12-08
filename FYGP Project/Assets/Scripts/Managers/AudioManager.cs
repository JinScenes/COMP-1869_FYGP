using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public class Sound
    {
        public string audioName;
        public AudioClip clip;
        public SoundType type;
        public bool is3D;
    }

    public enum SoundType
    {
        Music,
        SoundEffect,
        BGM,
    }

    [System.Serializable]
    public class SoundGroup
    {
        public string groupName;
        public List<Sound> sounds;
    }

    public List<SoundGroup> soundGroups;
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
            //foreach (var key in audioSources.Keys)
            //{
            //    Debug.Log("Audio source key: " + key);
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitialiseAudioSources()
    {
        audioSources = new Dictionary<string, AudioSource>();

        foreach (SoundGroup group in soundGroups)
        {
            foreach (Sound sound in group.sounds)
            {
                GameObject soundObject = new GameObject($"AudioSource_{sound.audioName}");
                soundObject.transform.SetParent(transform);

                AudioSource source = soundObject.AddComponent<AudioSource>();
                source.clip = sound.clip;
                source.playOnAwake = false;
                source.spatialBlend = sound.is3D ? 1 : 0;

                if (sound.audioName == "Background Music")
                {
                    source.loop = true;
                }

                audioSources.Add(sound.audioName, source);
            }
        }
    }


    private void LoadPlayerPrefs()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
        SetSoundEffectVolume(PlayerPrefs.GetFloat("SoundEffects", 5f));
        SetBGMVolume(PlayerPrefs.GetFloat("UISoundsVolume", 1f));
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
            int randomIndex = Random.Range(0, audioNames.Length);
            string randomAudioName = audioNames[randomIndex];
            PlayAudios(randomAudioName, pos);
        }
    }

    private void SetVolume(SoundType type, float volume)
    {
        foreach (SoundGroup group in soundGroups)
        {
            foreach (Sound sound in group.sounds)
            {
                if (sound.type == type && audioSources.ContainsKey(sound.audioName))
                {
                    audioSources[sound.audioName].volume = volume;
                }
            }
        }
    }
}
