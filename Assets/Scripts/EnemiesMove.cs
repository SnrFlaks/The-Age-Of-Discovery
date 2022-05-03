using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemiesMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private int z =250;
    void Update()
    {
      transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,transform.position.y),new Vector2(250,250),10 * Time.deltaTime  );
      transform.Rotate(0,0,100 * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Base")
        {
            Base.health -= 200;
            Base.hp.text = Base.health.ToString();
            if(Base.health <= 0){Destroy(col.gameObject);}
            Destroy(gameObject);
        }
    }
}
