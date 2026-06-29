using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMix;

    public void UpdateMasterVolume(float sliderValue)
    {
        if (masterMix == null)
            return;
        masterMix.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void UpdateSFXVolume(float sliderValue)
    {
        if (masterMix == null)
            return;
        masterMix.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void UpdateMusicVolume(float sliderValue)
    {
        if (masterMix == null)
            return;
        masterMix.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
}
