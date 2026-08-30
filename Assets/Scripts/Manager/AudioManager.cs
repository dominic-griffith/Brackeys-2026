using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public float MasterVolume { get; private set; } = 1f;

    //[SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private AudioMixerGroup _masterMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _SFXMixerGroup;
    [SerializeField] private Sound[] _sounds;

    //[Header("Cinemachine")]
    //[SerializeField] private CinemachineBrain _cinemachineBrain;
    private CinemachineBrain _cinemachineBrain;

    private ICinemachineCamera _previousCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in _sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;

            switch (sound.AudioType)
            {
                case Sound.AudioTypes.SFX:
                    sound.Source.outputAudioMixerGroup =
                        _SFXMixerGroup;
                    break;

                case Sound.AudioTypes.Music:
                    sound.Source.outputAudioMixerGroup =
                        _musicMixerGroup;
                    break;
            }
        }

        FindCinemachineBrain();
    }

    private void HandleSceneLoaded(
    Scene scene,
    LoadSceneMode loadMode)
    {
        _cinemachineBrain = null;
        _previousCamera = null;

        FindCinemachineBrain();
    }

    private void FindCinemachineBrain()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            _cinemachineBrain =
                mainCamera.GetComponent<CinemachineBrain>();
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    private void Update()
    {
        if (_cinemachineBrain == null)
        {
            FindCinemachineBrain();

            if (_cinemachineBrain == null)
            {
                return;
            }
        }

        ICinemachineCamera currentCamera =
            _cinemachineBrain.ActiveVirtualCamera;

        if (currentCamera != _previousCamera)
        {
            foreach (Sound sound in _sounds)
            {
                sound.Source.mute =
                    !CanPlayFromCurrentCamera(sound);
            }

            _previousCamera = currentCamera;
        }
    }


    public static AudioManager GetInstance()
    {
        return Instance;
    }

    private void PlayMusic()
    {
        //Play("Music");
    }

    //Play/Stop Audio Clip
    //ex use: AudioManager.GetInstance().Play("name");
    public void Play(string name)
    {
        Sound sound = FindSound(name);

        if (sound == null)
        {
            return;
        }

        if (!CanPlayFromCurrentCamera(sound))
        {
            Debug.Log(
                $"Sound '{name}' cannot play from the active camera.",
                this
            );

            return;
        }
        // Mute the sound if it is not allowed in the current camera
        sound.Source.mute = !CanPlayFromCurrentCamera(sound);
        sound.Source.Play();
    }

    public void Stop(string name)
    {
        Sound s = FindSound(name);
        if (s == null) return;
        s.Source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = FindSound(name);
        if (s == null) return;
        s.Source.Pause();
    }

    private Sound FindSound(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.Name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return null;
        }
        return s;
    }

    public void SetMasterVolume(float sliderValue)
    {
        MasterVolume = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float decibels = Mathf.Log10(MasterVolume) * 20f;

        _masterMixerGroup.audioMixer.SetFloat(
            "MasterExposed",
            decibels
        );
    }

    private bool CanPlayFromCurrentCamera(Sound sound)
    {
        if (sound.AllowedCameraNames == null ||
            sound.AllowedCameraNames.Length == 0)
        {
            return true;
        }

        if (_cinemachineBrain == null)
        {
            FindCinemachineBrain();
        }

        if (_cinemachineBrain == null ||
            _cinemachineBrain.ActiveVirtualCamera == null)
        {
            return false;
        }

        string activeCameraName =
            _cinemachineBrain.ActiveVirtualCamera.Name;

        foreach (string allowedCameraName
                 in sound.AllowedCameraNames)
        {
            if (string.Equals(
                    activeCameraName,
                    allowedCameraName,
                    StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    public float GetClipLength(string name)
    {
        Sound s = FindSound(name);
        if (s == null) return 0f;
        return s.Clip.length;
    }

    public void SetPan(string name, float panValue)
    {
        Sound s = FindSound(name);
        if (s == null) return;
        s.Source.panStereo = panValue;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            Instance = null;
        }
    }
}