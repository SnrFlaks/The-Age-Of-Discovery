using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class Zoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float sensivity;
    public static float cameraSize;

    private void Start()
    {
        sensivity = 25f;
    }

    void Update()
    {
        cameraSize = virtualCamera.m_Lens.OrthographicSize;
        
        if (virtualCamera.m_Lens.OrthographicSize == 1 )
        {
            if(Input.GetAxis("Mouse ScrollWheel") *  sensivity < 0){virtualCamera.m_Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel") * sensivity;}
        }
        else if (virtualCamera.m_Lens.OrthographicSize == 100 )
        {
            if(Input.GetAxis("Mouse ScrollWheel") *  sensivity > 0){virtualCamera.m_Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel") * sensivity;}
        }
        else if (virtualCamera.m_Lens.OrthographicSize < 1)
        {
            virtualCamera.m_Lens.OrthographicSize = 1;
        }
        else if (virtualCamera.m_Lens.OrthographicSize > 100)
        {
            virtualCamera.m_Lens.OrthographicSize = 100;
        }
        else{virtualCamera.m_Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel") * sensivity;}

    }
}