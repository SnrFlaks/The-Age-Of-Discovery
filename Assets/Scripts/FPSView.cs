using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class FPSView : MonoBehaviour
{
    [SerializeField] Pipes pipes;
    public static float fps;
    public static float totalExecutionTimeMs = 0;
    void OnGUI()
    {
        fps = 1.0f / Time.deltaTime;
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tFPS: " + (int)fps);
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tRESOURCE_NUMBER: " + pipes.resourceNumber);
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tMS: " + Sampler.Get("ResourceManager.Update() [Invoke]").GetRecorder().elapsedNanoseconds / 1000000.0f);
        // GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tMS.PIPE.AWAKE: " + Sampler.Get("Pipe.Awake").GetRecorder().elapsedNanoseconds / 1000000.0f);
        // GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tMS.PIPE.INITPART1: " + Sampler.Get("Pipe.InitPart1").GetRecorder().elapsedNanoseconds / 1000000.0f);
        // GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tMS.PIPE.INITPART1.NEIGHBORPIPES: " + Sampler.Get("Pipe.InitPart1.NeighborPipes").GetRecorder().elapsedNanoseconds / 1000000.0f);
        // GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tMS.PIPE.INITPART2: " + Sampler.Get("Pipe.InitPart2").GetRecorder().elapsedNanoseconds / 1000000.0f);
        // GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tMS.PIPE.CHANGETARGETPOSITION: " + Sampler.Get("Pipe.ChangeTargetPosition").GetRecorder().elapsedNanoseconds / 1000000.0f);
    }
}
