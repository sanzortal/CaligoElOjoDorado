using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    //objects and values
    [SerializeField] Slider slider;
    [SerializeField] float sliderValue;

   //get the sound value
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = slider.value;
    }

    //change the volume of all the sounds
    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("volumenAudio",sliderValue);
        AudioListener.volume = slider.value;
    }

    
    
}
