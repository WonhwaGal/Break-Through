using System.Threading.Tasks;
using UnityEngine;

public class ProgressSaver
{
    private readonly ProgressData _progressData;

    public ProgressSaver()
    {
        _progressData = ServiceLocator.Container.RequestFor<ProgressData>();
    }

    public async void SaveProgressData()
    {
        _progressData.MusicVolume = PlayerPrefs.HasKey(Constants.PrefsMusic) ?
            PlayerPrefs.GetInt(Constants.PrefsMusic) : (int)Constants.DefaultMusic;
        _progressData.SoundVolume = PlayerPrefs.HasKey(Constants.PrefsSound) ?
            PlayerPrefs.GetInt(Constants.PrefsSound) : (int)Constants.DefaultSound;
        _progressData.MouseSensitivity = PlayerPrefs.GetInt(Constants.PrefsSensitivity);

        await Task.Delay(50);
        string sJson = JsonUtility.ToJson(_progressData);
        PlayerPrefs.SetString(Constants.SaveGameKey, sJson);
        PlayerPrefs.Save();
    }
}