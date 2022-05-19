using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 500;
    private static GameObject nearest;
    
    

    void Update()
    {
        if(CannonShoot.enemy != null) {Move();}
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag =="Enemy")
        {
            other.gameObject.GetComponent<EnemiesMove>().hp -= damage;
          
            if (other.gameObject.GetComponent<EnemiesMove>().hp <= 0)
            {
                Enemies._allEnemies.Remove(other.gameObject);
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }

    

    private void Move()
    { 
        nearest = CannonShoot.enemy;

        var position1 = transform.position;
        var position2 =  nearest.transform.position;
        transform.position = Vector2.MoveTowards(new Vector2(position1.x,position1.y),new Vector2(position2.x,position2.y),15 * Time.deltaTime);
    }
    
    
}
