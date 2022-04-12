using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    private AudioSource audio;
    [SerializeField] private Text label;
    [SerializeField] private GameObject insideSettings;
  
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();

        if (PlayerPrefs.HasKey("Volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
            AudioListener.volume = volumeSlider.value;
        }
     
        
        if (PlayerPrefs.HasKey("Quality"))
        {
            QualitySettings.SetQualityLevel((int)PlayerPrefs.GetFloat("Quality"));
            label.text = PlayerPrefs.GetString("Label");
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
        }

    }

    private void Update()
    {
        LabelCheck();
    }

    private void LabelCheck()
    {
        if (insideSettings.activeSelf && label.text != PlayerPrefs.GetString("Label"))
        {
            label.text = PlayerPrefs.GetString("Label");
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save("Volume",volumeSlider.value);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        Save("Quality",index);
        PlayerPrefs.SetString("Label",label.text);
    }

    private void Save(String name, float value)
    {
        PlayerPrefs.SetFloat(name,value);
    }
   
}
