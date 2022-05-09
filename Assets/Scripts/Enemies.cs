using System;
using System.Collections;
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
    private int y = 0;


    void Start()
    {
        StartCoroutine(Spawn());
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
                Instantiate(enemies[intStage / 2], new Vector3(0, r.Next(0, 500)), Quaternion.identity, gameObject.transform);
                Instantiate(enemies[intStage / 2], new Vector3(r.Next(0, 500), 500, 0f), Quaternion.identity, gameObject.transform);
                Instantiate(enemies[intStage / 2], new Vector3(500, r.Next(0, 500)), Quaternion.identity, gameObject.transform);
                Instantiate(enemies[intStage / 2], new Vector3(r.Next(0, 500), 0, 0), Quaternion.identity, gameObject.transform);
            }

            if (intStage == 1)
            {
                Instantiate(enemies[intStage / 2], new Vector3(0, y = r.Next(0, 500), 0f), Quaternion.identity, gameObject.transform);
                Instantiate(enemies[intStage / 2], new Vector3(y = r.Next(0, 500), 500, 0f), Quaternion.identity, gameObject.transform);
                Instantiate(enemies[intStage / 2], new Vector3(y = 500, r.Next(0, 500)), Quaternion.identity, gameObject.transform);
                Instantiate(enemies[intStage / 2], new Vector3(y = r.Next(0, 500), 0, 0), Quaternion.identity, gameObject.transform);
            }

            stage.text = "Wave: " + intStage;
            intStage++;

            yield return new WaitForSeconds(5);
        }

    }
}

