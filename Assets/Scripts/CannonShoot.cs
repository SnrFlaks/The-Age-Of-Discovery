using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CannonShoot : MonoBehaviour
{
    public  static GameObject enemy;
    private Color color;
    private string g;

    private void Update()
    {
        if (Enemies._allEnemies != null)
        {
            enemy = Closest();
        }
       
    }
    
    private  GameObject Closest()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (GameObject gm in Enemies._allEnemies)
        {
            float distanceToEnemy = (gm.transform.position - transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = gm;
            }  
        }
        return closestEnemy;
    }
    
    private void OnMouseDown()
    {
       transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.2f);
    }

    private void OnMouseExit()
    { 
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.tag == "Cannon")
    //     {
    //         Debug.Log("here");
    //         Destroy(other.gameObject);
    //     }
    // }
}
