using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public GameObject enemy;
    void Start()
    {
        StartCoroutine(Spawn());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            Instantiate(enemy, new Vector3(-250f,0f,0f),Quaternion.identity);
            Instantiate(enemy, new Vector3(0f,250f,0f),Quaternion.identity);
            Instantiate(enemy, new Vector3(250f,0f,0f),Quaternion.identity);
            Instantiate(enemy, new Vector3(0f,-250f,0f),Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        
    }
}
