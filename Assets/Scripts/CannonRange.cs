using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonRange : MonoBehaviour
{
    [SerializeField] private int speedOfShoot;
    [SerializeField] private GameObject bullet;
    public static bool entered = false;
    private GameObject cannon;
    
    public static GameObject nearest;
    public static Vector3 position1;
    public static Vector3 position2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Enemy" && !entered)
        {
            entered = true;
            StartCoroutine("Spawn");
        }

    }

  
    IEnumerator Spawn()
    {
        position1 = transform.position;
        nearest = CannonShoot.Closest(position1);
        
        cannon = transform.parent.GetChild(0).gameObject;
         cannon.transform.up = nearest.transform.position - cannon.transform.position;
        
        Instantiate(bullet, new Vector3(position1.x, position1.y,position1.z), Quaternion.identity, gameObject.transform.parent);
        
        yield return new WaitForSeconds(speedOfShoot);
        entered = false;
        
    }



}
