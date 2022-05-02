using System;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
     public static bool createLockHub;
     public static int health;
     public static TextMesh hp;
     [SerializeField] private Slider hpSlider;
     private void OnMouseEnter() => createLockHub = true;
     private void OnMouseExit() => createLockHub = false;
    void Start()
    {
        health = 10000;
        hp = transform.GetChild(0).GetComponent<TextMesh>();
        hp.text = health.ToString();
    }

    private void Update() => hpSlider.value = Convert.ToInt32(hp.text);
}
