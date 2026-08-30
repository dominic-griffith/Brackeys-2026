using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError(
                "AudioManager was not found.",
                this
            );

            return;
        }

        // Display the persistent AudioManager's current volume
        // without invoking the Slider event.
        _volumeSlider.SetValueWithoutNotify(
            AudioManager.Instance.MasterVolume
        );
    }

    public void SetVolume(float sliderValue)
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError(
                "AudioManager was not found.",
                this
            );

            return;
        }

        AudioManager.Instance.SetMasterVolume(
            sliderValue
        );
    }
}
