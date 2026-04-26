using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;
    //Set the volume to the slider
    private void Start()
    {
       
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = masterVolume;
        SetMasterVolume();
    }
    public void SetMasterVolume()
    {
        float volume = volumeSlider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void fullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }


}
