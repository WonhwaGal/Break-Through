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

    public SettingsPrefsSaver(AudioMixer myMixer, Slider musicSlider, Slider soundSlider, Slider sensSlider)
    {
        _myMixer = myMixer;
        _musicSlider = musicSlider;
        _soundSlider = soundSlider;
        _sensSlider = sensSlider;
    }

    public Action<float> OnSensSliderChanged { get; private set; }

    public void Init()
    {
        _musicSlider.onValueChanged.AddListener(value => _myMixer.SetFloat("myMusic", value));
        _soundSlider.onValueChanged.AddListener(value => _myMixer.SetFloat("mySounds", value));
        _sensSlider.onValueChanged.AddListener(value => UpdateSensitivity());

        AssignValues();
        UpdateSensitivity();
    }

    public void AssignValues()
    {
        if (!PlayerPrefs.HasKey(Constants.PrefsMusic))
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
        _musicSlider.value = Constants.DefaultMusic;
        _soundSlider.value = Constants.DefaultSound;
    }

    public void SetDefaultGameplay()
    {
        _sensSlider.value = Constants.DefaultSens;
    }

    public void AudioApply()
    {
        PlayerPrefs.SetInt(Constants.PrefsMusic, (int)_musicSlider.value);
        PlayerPrefs.SetInt(Constants.PrefsSound, (int)_soundSlider.value);
        PlayerPrefs.Save();
    }

    public void GameplayApply()
    {
        PlayerPrefs.SetInt(Constants.PrefsSens, (int)_sensSlider.value);
        PlayerPrefs.Save();
    }

    public void GetAudioPrefs()
    {
        _musicSlider.value = PlayerPrefs.GetInt(Constants.PrefsMusic);
        _soundSlider.value = PlayerPrefs.GetInt(Constants.PrefsSound);
    }

    public void GetGameplayPrefs()
    {
        _sensSlider.value = PlayerPrefs.GetInt(Constants.PrefsSens);
    }

    private void UpdateSensitivity()
    {
        var sensMultiplier = Constants.MouseSensitivityStep * _sensSlider.value + Constants.MouseSensitivityStep * 2;
        GameEventSystem.Send<SensitivityEvent>(new SensitivityEvent(sensMultiplier));
    }

    public void Dispose()
    {
        _musicSlider.onValueChanged.RemoveAllListeners();
        _soundSlider.onValueChanged.RemoveAllListeners();
        _sensSlider.onValueChanged.RemoveAllListeners();
        GC.SuppressFinalize(this);
    }
}