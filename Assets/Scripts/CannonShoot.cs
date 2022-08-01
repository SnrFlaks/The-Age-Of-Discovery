using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CannonShoot : MonoBehaviour
{
    public static  GameObject Closest(Vector3 pos)
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (GameObject gm in Enemies._allEnemies)
        {
            float distanceToEnemy = (gm.transform.position - pos).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = gm;
            }  
        }
        return closestEnemy;
    }

}
