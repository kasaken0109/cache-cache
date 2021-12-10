using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PortalGimmicController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("完了に必要なMP")]
    private float m_requireMp = 500;

    [SerializeField]
    [Tooltip("人数ごとのタスクのゲージの増加割合")]
    private float[] m_upRate = new float[] {0,1f, 1.1f, 1.3f, 1.5f };

    [Tooltip("MP値")]
    float mp = 0;

    [SerializeField]
    [Tooltip("ゲージUI")]
    private Image m_ui = default;

    int witchCount = 0;
    Witch witch;
    SpriteRenderer m_sr;
    Color current;
    PhotonView m_view;

    private bool m_complete = false;
    public bool Complete => m_complete;
    // Start is called before the first frame update
    void Start()
    {
        m_view = GetComponent<PhotonView>();
        m_sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!witch) return;
        m_view.RPC(nameof(SetMP),RpcTarget.All);
    }
    [PunRPC]
    public void SetMP()
    {
        mp = mp + witch.GetMpSpeed * m_upRate[witchCount] >= m_requireMp ? m_requireMp : mp + witch.GetMpSpeed * m_upRate[witchCount];
        m_ui.fillAmount = mp / m_requireMp;
        m_sr.color = mp >= m_requireMp ? new Color(255,255,255,255) : m_sr.color;
        m_complete = mp >= m_requireMp ? true : false;
        if(mp >= m_requireMp)
        {
            m_sr.color = new Color(255, 255, 255, 255);
            m_complete = true;
            witch.UseTask = false;
            PortalGimmicManager.Instance.CheckTask();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Witch"))
        {
            witch = other.GetComponent<Witch>();
            if (other.GetComponent<PhotonView>().IsMine)
            {
                witch.UseTask = true;
            }
            witchCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Witch"))
        {
            witch = other.GetComponent<Witch>();
            if (other.GetComponent<PhotonView>().IsMine)
            {
                witch.UseTask = false;
            }
            witchCount--;   
        }
    }
}
