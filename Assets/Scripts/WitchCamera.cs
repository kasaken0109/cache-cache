using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WitchCamera : MonoBehaviour
{
    [Header("カメラの速度"), SerializeField]
    float m_speed = 3f;

    PhotonView m_view = default;
    bool m_spectating = false;

    public bool Spectating { get => m_spectating; set {m_spectating = value; } }

    void Start()
    {
        m_view = GetComponent<PhotonView>();
    }


    void FixedUpdate()
    {
        if (!m_view || !m_view.IsMine || m_spectating) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0|| v != 0)
        {
            transform.position += new Vector3(h / 10, v / 10);
        }
    }
}
