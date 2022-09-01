using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int bulletSpeed;
    private Vector3 pos1;
    private Vector3 pos2;

    private GameObject nearest;
    private GameObject cannon;
    
    private void Start()
    {
        nearest = CannonRange.enemy;
        
        cannon = transform.parent.GetChild(0).gameObject;
        cannon.transform.up = nearest.transform.position - cannon.transform.position;
    }

    void Update()
    {
        if (nearest != null)
        {
            Move();
        }
        else
        {
            Destroy(gameObject);
        }
       
    }
    
    private void Move()
    {
        pos1 = transform.position;
        pos2 = nearest.transform.position;
        
        transform.position = Vector2.MoveTowards(new Vector2(pos1.x,pos1.y),new Vector2(pos2.x,pos2.y),bulletSpeed * Time.deltaTime);
    }
}
