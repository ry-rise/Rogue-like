using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private readonly string SettingsFileName = "//SettingsData.json";
    private readonly string GameFileName = "//SaveData.json";
    [SerializeField] private AudioMixer audioMixer;
    private void Start()
    {
        if (File.Exists($"{Application.persistentDataPath}{SettingsFileName}") == true)
        {
            SettingsLoad();
        }
        if (File.Exists($"{Application.persistentDataPath}{GameFileName}")==false)
        {
            if (GameObject.Find("DataDeleteButton"))
            {
                GameObject.Find("DataDeleteButton").GetComponent<Button>().interactable = false;
            }
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
        string path = $"{Application.persistentDataPath}{SettingsFileName}";
        File.WriteAllText(path, json);
    }
    public void SettingsLoad()
    {
        string path = $"{Application.persistentDataPath}{SettingsFileName}";
        string json = File.ReadAllText(path);
        SettingsData restoreSettingsData = JsonUtility.FromJson<SettingsData>(json);
        audioMixer.SetFloat("MasterVol", restoreSettingsData.masterVolume);
        audioMixer.SetFloat("BGMVol", restoreSettingsData.bgmVolume);
        audioMixer.SetFloat("SEVol", restoreSettingsData.seVolume);
    }
    public void GameDataDelete()
    {
        File.Delete($"{Application.persistentDataPath}{GameFileName}");
        if (GameObject.Find("DataDeleteButton"))
        {
            GameObject.Find("DataDeleteButton").GetComponent<Button>().interactable = false;
        }
    }
}


    

