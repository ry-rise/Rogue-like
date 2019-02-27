using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    private readonly string FileName = "//SettinsData.json";
    [SerializeField] private AudioMixer audioMixer;
    private void Start()
    {
        if (File.Exists($"{Application.persistentDataPath}{FileName}") == true)
        {
            SettingsLoad();
        }
    }
    public void SettingsSave()
    {
        float MasterVolume;
        float BGMVolume;
        float SEVolume;
        bool MasVol = audioMixer.GetFloat("MasterVol", out MasterVolume);
        bool BGMVol = audioMixer.GetFloat("BGMVol", out BGMVolume);
        bool SEVol = audioMixer.GetFloat("SEVol", out SEVolume);
        SettingsData settingsData = new SettingsData()
        {
            masterVolume = MasterVolume,
            bgmVolume = BGMVolume,
            seVolume = SEVolume
        };
        string json = JsonUtility.ToJson(settingsData);
        string path = $"{Application.persistentDataPath}{FileName}";
        File.WriteAllText(path, json);
    }
    public void SettingsLoad()
    {
        string path = $"{Application.persistentDataPath}{FileName}";
        string json = File.ReadAllText(path);
        SettingsData restoreSettingsData = JsonUtility.FromJson<SettingsData>(json);
        audioMixer.SetFloat("MasterVol", restoreSettingsData.masterVolume);
        audioMixer.SetFloat("BGMVol", restoreSettingsData.bgmVolume);
        audioMixer.SetFloat("SEVol", restoreSettingsData.seVolume);
    }
}


    

