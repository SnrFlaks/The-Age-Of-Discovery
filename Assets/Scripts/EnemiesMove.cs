using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemiesMove : MonoBehaviour
{
    public  int hp;
    

    private void Update()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,transform.position.y),new Vector2(250,250),50 * Time.deltaTime  );
        transform.Rotate(0,0,100 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Base")
        {
            Base.health -= 200;
            Base.hp.text = Base.health.ToString();
            Enemies._allEnemies.Remove(gameObject);
            Destroy(gameObject);
            if(Base.health <= 0){Destroy(col.gameObject);}
        }
        else if(col.gameObject.tag == "Build")
        {
            Buildings._objectInGround.SetTile(Buildings._objectInGround.WorldToCell(col.GetContact(0).point),null);
            Enemies._allEnemies.Remove(gameObject);
        }
    }
}
