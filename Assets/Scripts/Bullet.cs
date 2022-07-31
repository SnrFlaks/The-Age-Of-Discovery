using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int bulletSpeed;
    public static GameObject nearest;
    private Vector3 position1;
    private Vector3 position2;
    private float time;

    void Update()
    {
        if(CannonShoot.enemy != null) {Move();}
    }
    
    
    private void Move()
    {
        time += Time.deltaTime;
        if (time > 3)
        {
            Destroy(gameObject);
            time = 0;
        }
        
        nearest = CannonShoot.enemy;
        position1 = transform.position;
        position2 =  nearest.transform.position;
        
        transform.position = Vector2.MoveTowards(new Vector2(position1.x,position1.y),new Vector2(position2.x,position2.y),bulletSpeed * Time.deltaTime);
    }
}
