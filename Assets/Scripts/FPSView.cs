using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSView : MonoBehaviour
{
    public static float fps;
    void OnGUI()
    {
        fps = 1.0f / Time.deltaTime;
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tFPS: " + (int)fps);
    }
}
