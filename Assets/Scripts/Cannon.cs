using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public static Animator _animator;
    private Quaternion a;
    private Quaternion b;
    void Start()
    {
        _animator= GetComponent<Animator>();
    }
    
}
