using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;
    

    //get the volume of the game
    private void Start()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = masterVolume;
        SetMasterVolume();
    }

    //change the volume of all the game acording to the preferences of the player
    public void SetMasterVolume()
    {
        float volume = volumeSlider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    //activate/deactivate full screen
    public void fullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }


}
