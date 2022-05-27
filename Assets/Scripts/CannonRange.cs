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
        var pos = transform.position;
        
        cannon = transform.parent.GetChild(0).gameObject;
        cannon.transform.up = CannonShoot.enemy.transform.position - cannon.transform.position;
        
        Instantiate(bullet, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity, gameObject.transform.parent);
        if(Bullet.nearest != null)
        
        yield return new WaitForSeconds(speedOfShoot);
        entered = false;
        
    }



}
