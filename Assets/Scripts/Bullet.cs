using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    void Start()
    {
        
    }
    
    void Update()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x,transform.position.y),new Vector2(250,250),50 * Time.deltaTime  );
    }
}
