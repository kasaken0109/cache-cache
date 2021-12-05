using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Cinema : MonoBehaviour
{
    CinemachineVirtualCamera m_camera;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<CinemachineVirtualCamera>();
        m_camera.m_Lens.NearClipPlane = -12;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
