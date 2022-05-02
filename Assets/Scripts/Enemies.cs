
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class Enemies : MonoBehaviour
{
    [SerializeField] GameObject whiteCircle;
    private Random r = new Random();
    
    void Start()
    {
        StartCoroutine(Spawn());
    }
    
    IEnumerator Spawn()
    {
        while (true)
        {
            Instantiate(whiteCircle, new Vector3(0, r.Next(0, 500)), Quaternion.identity).transform.parent = gameObject.transform;
            Instantiate(whiteCircle, new Vector3(r.Next(0,500),500,0f),Quaternion.identity).transform.parent = gameObject.transform;
            Instantiate(whiteCircle, new Vector3(500,r.Next(0,500)),Quaternion.identity).transform.parent = gameObject.transform;
            Instantiate(whiteCircle, new Vector3(r.Next(0,500),0,0),Quaternion.identity).transform.parent = gameObject.transform;
            
            yield return new WaitForSeconds(1);
        }
        
    }
}

