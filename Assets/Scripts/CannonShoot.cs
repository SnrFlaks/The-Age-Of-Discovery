using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour
{
    public static bool createLockCannon;
    private void OnMouseEnter() => createLockCannon = true;
    private void OnMouseExit() => createLockCannon = false;
    [SerializeField] private int time;
    [SerializeField] private GameObject bullet;
    void Start()
    {
        StartCoroutine(Spawn(time));
    }

    IEnumerator Spawn(int time)
    {
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z),Quaternion.identity,gameObject.transform);
        yield return new WaitForSeconds(time);
    }
}
