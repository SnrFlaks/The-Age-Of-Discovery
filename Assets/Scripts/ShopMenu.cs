using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    private Slider _slider;
    private Text _text;
    private Text _btext;
    public Text tokens;
    public static int intTokens = 2000;

    void Start()
    {
        _slider = transform.GetChild(0).GetComponent<Slider>();
        _text = transform.GetChild(1).GetComponent<Text>();
        _btext = transform.GetChild(2).GetChild(0).GetComponent<Text>();
    }

    private void Update() => tokens.text = intTokens.ToString();

    public void AmountOfOre(float ore)
    {
        if (ore == 0)
        {
            _slider.maxValue = Buildings._tin + 0.0001f;
            _text.text = "" + Math.Round(_slider.value);
            _btext.text = "You will get: \n" + Mathf.Round(_slider.value) + " Tokens";
        }
        else if (ore == 1)
        {
            _slider.maxValue = Buildings._iron + 0.0001f;
            _text.text = "" + Math.Round(_slider.value);
            _btext.text = "You will get: \n" + Mathf.Round(_slider.value * 2) + " Tokens";
        }
        else if (ore == 2)
        {
            _slider.maxValue = Buildings._copper + 0.0001f;
            _text.text = "" + Math.Round(_slider.value);
            _btext.text = "You will get: \n" + Mathf.Round(_slider.value * 4) + " Tokens";
        }
        else if (ore == 3)
        {
            _slider.maxValue = Buildings._gold + 0.0001f;
            _text.text = "" + Math.Round(_slider.value);
            _btext.text = "You will get: \n" + Mathf.Round(_slider.value * 8) + " Tokens";
        }
    }

    public void Sell(int ore)
    {
        if (ore == 0 && _slider.value < Buildings._tin)
        {
            intTokens += Convert.ToInt32(Regex.Match(_btext.text, @"\d+").Value);
            tokens.text = "Tokens: \n" + intTokens;
            Buildings._tin -= _slider.value;

        }
        else if (ore == 1 && _slider.value < Buildings._iron)
        {
            intTokens += Convert.ToInt32(Regex.Match(_btext.text, @"\d+").Value);
            tokens.text = "Tokens: \n" + intTokens;
            Buildings._iron -= _slider.value;

        }
        else if (ore == 2 && _slider.value < Buildings._copper)
        {
            intTokens += Convert.ToInt32(Regex.Match(_btext.text, @"\d+").Value);
            tokens.text = "Tokens: \n" + intTokens;
            Buildings._copper -= _slider.value;

        }
        else if (ore == 3 && _slider.value < Buildings._gold)
        {
            intTokens += Convert.ToInt32(Regex.Match(_btext.text, @"\d+").Value);
            tokens.text = "Tokens: \n" + intTokens;
            Buildings._gold -= _slider.value;

        }
    }
}
