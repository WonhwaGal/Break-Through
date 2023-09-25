using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPrefsSaver : IDisposable
{
    private readonly AudioMixer _myMixer;
    private readonly Slider _musicSlider;
    private readonly Slider _soundSlider;
    private readonly Slider _sensSlider;

    private const string PrefsMusic = "masterMusic";
    private const string PrefsSound = "masterSounds";
    private const string PrefsSens = "masterSensitivity";

    private const float DefaultMusic = -25;
    private const float DefaultSound = -10;
    private const float DefaultSens = 3;

    public SettingsPrefsSaver(AudioMixer myMixer, Slider musicSlider, Slider soundSlider, Slider sensSlider)
    {
        _myMixer = myMixer;
        _musicSlider = musicSlider;
        _soundSlider = soundSlider;
        _sensSlider = sensSlider;
    }

    public event Action<float> OnSensSliderChanged;

    public void Init()
    {
        _musicSlider.onValueChanged.AddListener(value => _myMixer.SetFloat("myMusic", value));
        _soundSlider.onValueChanged.AddListener(value => _myMixer.SetFloat("mySounds", value));
        _sensSlider.onValueChanged.AddListener(value => OnSensSliderChanged?.Invoke(value));

        AssignValues();
    }

    public void AssignValues()
    {
        if (!PlayerPrefs.HasKey(PrefsMusic))
        {
            SetDefaultAudio();
            SetDefaultGameplay();
        }
        else
        {
            GetAudioPrefs();
            GetGameplayPrefs();
        }
    }

    public void SetDefaultAudio()
    {
        _musicSlider.value = DefaultMusic;
        _soundSlider.value = DefaultSound;
    }

    public void SetDefaultGameplay()
    {
        _sensSlider.value = DefaultSens;
    }

    public void AudioApply()
    {
        PlayerPrefs.SetInt(PrefsMusic, (int)_musicSlider.value);
        PlayerPrefs.SetInt(PrefsSound, (int)_soundSlider.value);
    }

    public void GameplayApply()
    {
        PlayerPrefs.SetInt(PrefsSens, (int)_sensSlider.value);
    }

    public void GetAudioPrefs()
    {
        _musicSlider.value = PlayerPrefs.GetInt(PrefsMusic);
        _soundSlider.value = PlayerPrefs.GetInt(PrefsSound);
    }

    public void GetGameplayPrefs()
    {
        _sensSlider.value = PlayerPrefs.GetInt(PrefsSens);
    }

    public void Dispose()
    {
        _musicSlider.onValueChanged.RemoveAllListeners();
        _soundSlider.onValueChanged.RemoveAllListeners();
        _sensSlider.onValueChanged.RemoveAllListeners();
        GC.SuppressFinalize(this);
    }
}