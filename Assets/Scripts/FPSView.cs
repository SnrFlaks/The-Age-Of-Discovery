using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class FPSView : MonoBehaviour
{
    [SerializeField] PipeManager pipes;
    public static float fps;
    public static float totalExecutionTimeMs = 0;
    public static float wg1;
    public static Vector3Int cellPosition;
    void OnGUI()
    {
        fps = 1.0f / Time.deltaTime;
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tFPS: " + (int)fps);
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tRESOURCE_NUMBER: " + pipes.resourceNumber);
        //GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tMS: " + Sampler.Get("ResourceManager.Update() [Invoke]").GetRecorder().elapsedNanoseconds / 1000000.0f);
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tWORLD_GENERATION: " + wg1);
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tCELL_POSITION: " + " X: " + cellPosition.x + " Y: " + cellPosition.y + " Z: " + cellPosition.z);
    }
}
