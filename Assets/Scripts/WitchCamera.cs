using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WitchCamera : MonoBehaviour
{
    [Header("カメラの速度"), SerializeField]
    float m_speed = 3f;

    Transform m_witch = default;
    public Transform WitchT { get => m_witch; set { m_witch = value; } }


    public void CameraMove(float h)
    {
        if (h != 0)
        {
            m_witch.position += new Vector3(h / 10, 0);
        }
    }
}
