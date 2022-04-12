using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class Zoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float sensivity;
    

    private void Start()
    {
        sensivity = 5f;
    }

    void Update()
    {
        
        if (virtualCamera.m_Lens.OrthographicSize == 1 )
        {
            if(Input.GetAxis("Mouse ScrollWheel") *  sensivity < 0){virtualCamera.m_Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel") * sensivity;}
        }
        else if (virtualCamera.m_Lens.OrthographicSize == 10 )
        {
            if(Input.GetAxis("Mouse ScrollWheel") *  sensivity > 0){virtualCamera.m_Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel") * sensivity;}
        }
        else{virtualCamera.m_Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel") * sensivity;}

    }
}