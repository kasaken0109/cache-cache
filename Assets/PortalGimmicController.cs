using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PortalGimmicController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ギミックの個数")]
    private int gimmicNum = 5;
    [SerializeField]
    [Tooltip("ギミックのHP")]
    private int[] m_gimmicHP = new int[] {7,7,7,7,7};

    [SerializeField]
    [Tooltip("破壊時のエフェクト")]
    private GameObject m_destroyEffect = default;

    [Tooltip("MP値")]
    float mp = 0;

    [SerializeField]
    [Tooltip("体力UI")]
    private SpriteRenderer[] m_ui = default;

    [SerializeField]
    [Tooltip("本物のギミックかどうか")]
    private bool m_isGenuine = true;

    public bool IsGenuine { get => m_isGenuine; set { m_isGenuine = value; } }

    SpriteRenderer m_sr;
    PhotonView m_view;

    private bool m_complete = false;
    public bool Complete => m_complete;
    // Start is called before the first frame update
    void Start()
    {
        m_view = GetComponent<PhotonView>();
        m_sr = GetComponent<SpriteRenderer>();

    }

    int index = 0;
    [PunRPC]
    public void SetMP()
    {
        if (!IsGenuine)
        {
            foreach (var item in m_ui)
            {
                item.enabled = false;
                m_sr.material.color = new Color(0, 0, 0, 255);
                Instantiate(m_destroyEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation);
            }
        }
        if (m_gimmicHP[index] == 1)
        {
            if(index == gimmicNum - 1)
            {
                m_sr.color = new Color(255, 255, 255, 255);
                m_complete = true;
                PortalGimmicManager.Instance.CheckTask();
                m_ui[index].enabled = false;
                Instantiate(m_destroyEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.rotation);
                return;
            }
            m_ui[index].enabled = false;
            index++;
        }
        Instantiate(m_destroyEffect,new Vector3(transform.position.x,transform.position.y,transform.position.z - 1), transform.rotation);
        m_gimmicHP[index]--;
    }

    public void Damage()
    {
        m_view.RPC(nameof(SetMP), RpcTarget.All);
    }
}
