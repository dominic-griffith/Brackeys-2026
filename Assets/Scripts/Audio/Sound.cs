using Unity.Cinemachine;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    public AudioTypes AudioType;

    [Range(0f, 1f)]
    public float Volume = 1f;

    [Range(0.1f, 3f)]
    public float Pitch = 1f;

    public bool Loop;

    [Header("Camera Restrictions")]
    [Tooltip("Leave empty to allow this sound from every camera.")]
    //public CinemachineCamera[] AllowedCameras;
    public string[] AllowedCameraNames;

    [HideInInspector]
    public AudioSource Source;

    public enum AudioTypes
    {
        SFX,
        Music
    }
}