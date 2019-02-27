using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public sealed class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    private float MVol;
    private float BVol;
    private float SVol;

    private void Start()
    {
        if (audioMixer.GetFloat("MasterVol", out MVol))
        {
            masterSlider.value = MVol;
        }
        if (audioMixer.GetFloat("BGMVol", out BVol))
        {
            bgmSlider.value = BVol;
        }
        if (audioMixer.GetFloat("SEVol", out SVol))
        {
            seSlider.value = SVol;
        }
    }
    public void SetMaster(float volume)
    {
        audioMixer.SetFloat("MasterVol", volume);
    }
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMVol", volume);
    }
    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEVol", volume);
    }
}
