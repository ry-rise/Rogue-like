using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Button dataDeleteButton;
    [SerializeField] private Button backToTitleButton;

    private void Start()
    {
        dataDeleteButton.onClick.AddListener(DataManager.GameDataDelete);
        backToTitleButton.onClick.AddListener(() => DataManager.SettingsDataSave(audioMixer));
        backToTitleButton.onClick.AddListener(SceneChanger.ToTitle);
        if (File.Exists($"{Application.persistentDataPath}{DataManager.SettingsFileName}") == true)
        {
            DataManager.SettingsDataLoad(audioMixer);
        }
        if (File.Exists($"{Application.persistentDataPath}{DataManager.GameFileName}") == false)
        {
            dataDeleteButton.interactable = false;
        }
    }

}
