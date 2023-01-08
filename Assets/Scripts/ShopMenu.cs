using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private ResourcesData resourcesData;
    [SerializeField] private Buildings buildings;
    private Slider _slider;
    private Text _text;
    private Text _btext;
    public Text tokensText;
    public static Text tokens;

    public static int intTokens = 100000000;

    void Start()
    {
        _slider = transform.GetChild(0).GetComponent<Slider>();
        _text = transform.GetChild(1).GetComponent<Text>();
        _btext = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        tokens = tokensText;
    }
    public void AmountOfOre(float ore)
    {
        for (int i = 0; i < 4; i++)
        {
            if (ore == i)
            {
                _slider.maxValue = resourcesData._oreArray[i] + 0.0001f;
                _text.text = "" + Math.Round(_slider.value);
                _btext.text = "You will get: \n" + Mathf.Round(_slider.value) + " Tokens";
            }
        }
    }

    public void Sell(int ore)
    {
        for (int i = 0; i < 4; i++)
        {
            if (ore == i && _slider.value < resourcesData._oreArray[i])
            {
            intTokens += Convert.ToInt32(Regex.Match(_btext.text, @"\d+").Value);
            tokens.text = "Tokens: \n" + intTokens;
            resourcesData._oreArray[i] -= Convert.ToInt32(_slider.value);
            }
        }
    }
}
