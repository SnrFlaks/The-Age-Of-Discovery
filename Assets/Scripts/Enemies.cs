using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Enemies : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    [SerializeField] private Text timeUntilNewStage;
    private float time;

    [SerializeField] private Text stage;
    private int intStage = 0;

    private Random r = new Random();
    [SerializeField] private bool ifSpawn = false;
    public static List<GameObject> _allEnemies = new List<GameObject>(25);
    
    


    void Start()
    {
        
        if (ifSpawn) {StartCoroutine(Spawn()); }
        time = 5;
    }

    private void Update()
    {
        time -= time < 0 ? -5 : Time.deltaTime;
        timeUntilNewStage.text = "New wave in: " + Mathf.Round(time);
    }

    IEnumerator Spawn()
    {
        while (true)
        {

            for (int i = 0; i < intStage / 2; i++)
            {
                _allEnemies.Add(Instantiate(enemies[intStage / 2], new Vector3(0, r.Next(0, 500), 0f), Quaternion.identity, gameObject.transform));
                _allEnemies.Add(Instantiate(enemies[intStage / 2], new Vector3( r.Next(0, 500), 500, 0f), Quaternion.identity, gameObject.transform));
                _allEnemies.Add(Instantiate(enemies[intStage / 2], new Vector3(  500, r.Next(0, 500)), Quaternion.identity, gameObject.transform));
                _allEnemies.Add(Instantiate(enemies[intStage / 2], new Vector3(r.Next(0, 500), 0, 0), Quaternion.identity, gameObject.transform));
            }

            if (intStage == 1)
            {
                _allEnemies.Add(Instantiate(enemies[intStage / 2], new Vector3(0, r.Next(0, 500), 0f), Quaternion.identity, gameObject.transform));
                 _allEnemies.Add(Instantiate(enemies[intStage / 2], new Vector3( r.Next(0, 500), 500, 0f), Quaternion.identity, gameObject.transform));
                 _allEnemies.Add(Instantiate(enemies[intStage / 2], new Vector3(  500, r.Next(0, 500)), Quaternion.identity, gameObject.transform));
                 _allEnemies.Add(Instantiate(enemies[intStage / 2], new Vector3(r.Next(0, 500), 0, 0), Quaternion.identity, gameObject.transform));
            }

            stage.text = "Wave: " + intStage;
            intStage++;

            yield return new WaitForSeconds(5);
        }

    }
 
}

