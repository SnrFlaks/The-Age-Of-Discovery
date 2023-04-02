using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;
using UnityEngine.Profiling;

public class Resource : MonoBehaviour
{
    public Vector3 dataPos { get; set; }
    public Vector3 targetPosition { get; set; }
    public bool isVisible { get; set; }
    public int lastDirection { get; set; }
    public Pipe startPipe { get; set; }
    public Pipe currentPipe { get; set; }
    public Pipe lastPipe { get; set; }
    public bool isStart { get; set; }
    public Transform[] neighboringPipes { get; set; }
    public Pipe[] neighboringPipesComponent { get; set; }
    private bool isAwake;
    private void Awake()
    {
        isVisible = true;
        dataPos = transform.localPosition;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    void OnBecameVisible()
    {
        isVisible = true;
        if (isAwake) transform.localPosition = dataPos;
        else isAwake = true;
    }
    void OnBecameInvisible()
    {
        isVisible = false;
        dataPos = transform.localPosition;
    }
}
