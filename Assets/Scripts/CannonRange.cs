using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class CannonRange : MonoBehaviour
{
    [SerializeField] private int speedOfShoot;
    [SerializeField] private GameObject bullet;
    
    private static Vector3 position1;
    private object myLock = new object();
    public static GameObject enemy;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && !Monitor.IsEntered(myLock))
        {
             enemy = other.gameObject;
             other.GetComponent<EnemiesMove>().TheoreticalHp -= bullet.GetComponent<Bullet>().damage;
            if (other.GetComponent<EnemiesMove>().TheoreticalHp <= 0)
            {
                other.tag = "Untagged";
            }
            StartCoroutine(Spawn());
        }
    }
    
    IEnumerator Spawn()
    {
        Monitor.TryEnter(myLock);
        position1 = transform.position;
        Instantiate(bullet, new Vector3(position1.x, position1.y, position1.z), Quaternion.identity, gameObject.transform.parent);
        
        yield return new WaitForSeconds(speedOfShoot);
        Monitor.Exit(myLock);
    }
}






