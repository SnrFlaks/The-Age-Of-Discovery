using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int bulletSpeed;
    [HideInInspector] public GameObject nearest;

    private Vector3 pos1;
    private Vector3 pos2;
    private GameObject cannon;

    private void Start()
    {
        nearest = gameObject.transform.parent.GetChild(1).gameObject.GetComponent<CannonRange>().enemy;

        cannon = transform.parent.GetChild(0).gameObject;
        cannon.transform.up = nearest.transform.position - cannon.transform.position;
    }

    void Update()
    {
        if (nearest != null)
        {
            Move();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        pos1 = transform.position;
        pos2 = nearest.transform.position;

        transform.position = Vector2.MoveTowards(new Vector2(pos1.x, pos1.y), new Vector2(pos2.x, pos2.y), bulletSpeed * Time.deltaTime);
    }
}
