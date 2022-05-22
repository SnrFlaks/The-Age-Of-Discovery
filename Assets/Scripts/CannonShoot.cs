using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CannonShoot : MonoBehaviour
{
    public static bool createLockCannon;
    private void OnMouseEnter() => createLockCannon = true;
    private void OnMouseExit() => createLockCannon = false;
    public  static GameObject enemy;
    private Color color;

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
        color = transform.GetChild(1).GetComponent<SpriteRenderer>().color = color.a == 0.2f ? new Color(1,1,1,0f): new Color(1,1,1,0.2f);
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
