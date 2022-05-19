using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonRange : MonoBehaviour
{
    [SerializeField] private int time;
     [SerializeField] private GameObject bullet;
     private bool entered = false;
    
    
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
         Instantiate(bullet, new Vector3(pos.x, pos.y, pos.z),Quaternion.identity,gameObject.transform.parent);
         yield return new WaitForSeconds(3);
         entered = false;
     }


     private void OnTriggerExit2D(Collider2D other)
     {
         
         if (other.gameObject.tag == "Bullet" && transform.parent.childCount>2 && other.gameObject == transform.parent.GetChild(2).gameObject )
         {
             Destroy(other.gameObject);
         }
     }

    
}
