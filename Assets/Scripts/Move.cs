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
        _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * _speed * Time.fixedDeltaTime * Zoom.cameraSize / 7.5f, 
             Input.GetAxis("Vertical") * _speed * Time.fixedDeltaTime * Zoom.cameraSize /7.5f);
         
         transform.position = new Vector3(Mathf.Clamp(transform.position.x, 5, 495),
             Mathf.Clamp(transform.position.y, 5, 495), transform.position.z);

    }
}