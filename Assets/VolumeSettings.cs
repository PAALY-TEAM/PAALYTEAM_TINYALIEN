using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    private const string _mixerMaster = "MasterVolume";
    private const string _mixerMusic = "MusicVolume";
    private const string _mixerAmbiance = "AmbianceVolume";
    private const string _mixerSfx = "SfxVolume";
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambianceSlider;
    [SerializeField] private Slider sfxSlider;


    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(delegate { SetMasterVolume(masterSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(musicSlider.value); });
        ambianceSlider.onValueChanged.AddListener(delegate { SetAmbianceVolume(ambianceSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSfxVolume(sfxSlider.value); });
    }

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        ambianceSlider.value = PlayerPrefs.GetFloat("AmbianceVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }
    
    private void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(_mixerMusic, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(_mixerMusic, volume);
    }
    
    private void SetAmbianceVolume(float volume)
    {
        audioMixer.SetFloat(_mixerAmbiance, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(_mixerAmbiance, volume);
    }

    private void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(_mixerMaster, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(_mixerMaster, volume);
    }
    
    private void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat(_mixerSfx, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(_mixerSfx, volume);
    }
}