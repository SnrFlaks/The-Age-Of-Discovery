using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private AudioSource aS;
    
    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void Sound()
    {
        
        aS.Play();
    }
}
