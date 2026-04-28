using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShineOptions : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float sliderValue;
    [SerializeField] Image panel;


    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("brillo", 0.5f);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, slider.value);
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("brillo", sliderValue);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, slider.value);
    }
   
    void Update()
    {
        
    }
}
