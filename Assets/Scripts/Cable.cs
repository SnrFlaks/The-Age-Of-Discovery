using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    private int[][] info = new int[500][];
    public static int countRow;
    public static int[] rowArr;
    void Start()
    {
        for (var i = 0; i < info.Length; i++) info[i] = new int[500];
    }

    IEnumerator OncePerSeconds()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(1);
        }
    }
}
