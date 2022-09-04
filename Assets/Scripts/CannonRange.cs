using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class CannonRange : MonoBehaviour
{
    [SerializeField] private int TimeForShoot;
    [SerializeField] private GameObject bullet;
    [HideInInspector] public GameObject enemy;
    
    private Vector3 position1;
    private object myLock = new object();
    
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
        
        yield return new WaitForSeconds(TimeForShoot);
        Monitor.Exit(myLock);
    }
}






