using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using Unity.Cinemachine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //[SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private AudioMixerGroup _masterMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _SFXMixerGroup;
    [SerializeField] private Sound[] _sounds;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineBrain _cinemachineBrain;

    private ICinemachineCamera _previousCamera;

    private void Awake()
    {
        //Singleton Design Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }


        //Assign atributes to the sound
        foreach (Sound s in _sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;

            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;

            switch (s.AudioType)
            {
                case (Sound.AudioTypes.SFX):
                    s.Source.outputAudioMixerGroup = _SFXMixerGroup;
                    break;
                case (Sound.AudioTypes.Music):
                    s.Source.outputAudioMixerGroup = _musicMixerGroup;
                    break;
            }
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    private void Update()
    {
        // Skip if the brain hasn't been found yet
        if (_cinemachineBrain == null) return;

        ICinemachineCamera currentCamera = _cinemachineBrain.ActiveVirtualCamera;

        // Only process audio checks on the exact frame the camera changes
        if (currentCamera != _previousCamera)
        {
            foreach (Sound sound in _sounds)
            {
                sound.Source.mute = !CanPlayFromCurrentCamera(sound);
            }

            // Update the tracker
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
        float volume = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float decibels = Mathf.Log10(volume) * 20f;

        _masterMixerGroup.audioMixer.SetFloat(
            "MasterExposed",
            decibels
        );
    }

    private bool CanPlayFromCurrentCamera(Sound sound)
    {
        // An empty list allows the sound to play from every camera.
        if (sound.AllowedCameras == null ||
            sound.AllowedCameras.Length == 0)
        {
            return true;
        }

        if (_cinemachineBrain == null)
        {
            Camera mainCamera = Camera.main;

            if (mainCamera != null)
            {
                _cinemachineBrain =
                    mainCamera.GetComponent<CinemachineBrain>();
            }
        }

        if (_cinemachineBrain == null)
        {
            Debug.LogWarning(
                "CinemachineBrain could not be found.",
                this
            );

            return false;
        }

        foreach (CinemachineCamera allowedCamera
                in sound.AllowedCameras)
        {
            if ((allowedCamera != null) && ((_cinemachineBrain.ActiveVirtualCamera as CinemachineCamera) == allowedCamera))
            {
                return true;
            }
        }

        return false;
    }

}