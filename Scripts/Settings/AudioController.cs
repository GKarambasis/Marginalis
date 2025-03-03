using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider, sfxSlider;

    [SerializeField] AudioMixer musicMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("master")) { masterSlider.value = PlayerPrefs.GetFloat("master"); }
        if (PlayerPrefs.HasKey("music")) { musicSlider.value = PlayerPrefs.GetFloat("music"); }
        if (PlayerPrefs.HasKey("sfx")) { sfxSlider.value = PlayerPrefs.GetFloat("sfx"); }

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        musicMixer.SetFloat("master", Mathf.Log10(volume)*20);

        PlayerPrefs.SetFloat("master", volume);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        musicMixer.SetFloat("music", Mathf.Log10(volume)*20);

        PlayerPrefs.SetFloat("music", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        musicMixer.SetFloat("sfx", Mathf.Log10(volume)*20);

        PlayerPrefs.SetFloat("sfx", volume);
    }
}
