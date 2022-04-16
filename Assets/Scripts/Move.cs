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
        transform.position = new Vector3(22.2f,11.6f,0f);
    }


    private void FixedUpdate()
    {
        Moving();
    }

    private void Moving()
    {
        _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * _speed * Time.fixedDeltaTime * Zoom.cameraSize / 7.5f, 
            Input.GetAxis("Vertical") * _speed * Time.fixedDeltaTime * Zoom.cameraSize /7.5f);
    }
}