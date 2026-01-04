using System;
using UnityEngine;

[Serializable]
public class SettingsData
{
    [SerializeField] private float masterVolume;
    public float MasterVolume { get { return masterVolume; } set { masterVolume = value; } }
    [SerializeField] private float BGMVolume;
    public float bgmVolume { get { return BGMVolume; } set { BGMVolume = value; } }
    [SerializeField] private float SEVolume;
    public float seVolume { get { return SEVolume; } set { SEVolume = value; } }
    [SerializeField] private string bindingOverridesJson;
    public string BindingOverridesJson { get { return bindingOverridesJson; } set { bindingOverridesJson = value; } }
}
