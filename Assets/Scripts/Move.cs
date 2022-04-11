using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Move : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _speed;

   private void Start()
    {
        _speed = 1500f;
        _rb = GetComponent<Rigidbody2D>();
    }

    
    private void FixedUpdate()
    {
        Moving();
    }

    private void Moving()
    {
        _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * _speed * Time.fixedDeltaTime,Input.GetAxis("Vertical") * _speed * Time.fixedDeltaTime);
    }
    
}