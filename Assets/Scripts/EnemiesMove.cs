using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemiesMove : MonoBehaviour
{
    [SerializeField] private int hp;
    public int TheoreticalHp;
    [SerializeField] private int enemySpeed;
    
    private int damage;
    
    private void Start()
    {
        hp = GetComponent<EnemiesMove>().hp;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
            new Vector2(250, 250), enemySpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Base")
        {
            Base.health -= 200;
            Base.hp.text = Base.health.ToString();
            if (Base.health <= 0)
            {
                Destroy(col.gameObject);
            }
            
            Destroy(gameObject);
        }
        else if (col.gameObject.tag == "Build")
        {
            Buildings._objectInGround.SetTile(Buildings._objectInGround.WorldToCell(col.GetContact(0).point), null);
        }
        else if (col.gameObject.tag == "Cannon")
        {
            Buildings.cannonBoolArr[(int) col.transform.position.x][(int) col.transform.position.y] = false;
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            if (other.gameObject.GetComponent<Bullet>().nearest == gameObject)
            {
                damage = other.GetComponent<Bullet>().damage;
                hp -= damage;
            
                Destroy(other.gameObject);
                if (hp <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
