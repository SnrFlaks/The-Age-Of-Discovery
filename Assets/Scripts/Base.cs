using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
     public static bool createLockHub;
     public static int health;
     public static TextMesh hp;
     private void OnMouseEnter() => createLockHub = true;
     private void OnMouseExit() => createLockHub = false;
    void Start()
    {
        health = 10000;
        hp = transform.GetChild(0).GetComponent<TextMesh>();
        hp.text = health.ToString();
    }
    
}
