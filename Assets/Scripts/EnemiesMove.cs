using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesMove : MonoBehaviour
{
    private Rigidbody2D rb;
    void Update()
    {
      transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,transform.position.y),new Vector2(250,250),50 * Time.deltaTime  );
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
