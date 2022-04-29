using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
     public static bool createLockHub;
     private int health;
     private TextMesh hp;
     private void OnMouseEnter() => createLockHub = true;
     private void OnMouseExit() => createLockHub = false;
    void Start()
    {
        health = 10000;
        hp = transform.GetChild(0).GetComponent<TextMesh>();
        hp.text = health.ToString();
    }
    
}
