using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
public class Cinema : MonoBehaviour
{
    CinemachineVirtualCamera m_camera;
    PhotonView m_view;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<CinemachineVirtualCamera>();
        m_camera.m_Lens.NearClipPlane = -12;
        m_view = GetComponentInParent<PhotonView>();
        StartCoroutine(nameof(SetPriority));
    }

    IEnumerator SetPriority()
    {
        yield return new WaitForSeconds(1f);
        m_camera.Priority = m_view.IsMine ? 15 : 10;
    }
}
