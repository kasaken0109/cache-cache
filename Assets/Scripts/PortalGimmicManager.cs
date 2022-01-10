using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class PortalGimmicManager : MonoBehaviour
{
    public static PortalGimmicManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("ギミック")]
    PortalGimmicController[] m_gimmics;

    [SerializeField]
    [Tooltip("発生させる出口")]
    GameObject m_gate = default;

    [SerializeField]
    [Tooltip("減少させる時間")]
    int m_decreaseTime = 120;

    PhotonView m_view;
    PortalGimmicController[] gimmic;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_view = GetComponent<PhotonView>();
        gimmic = GameObject.FindObjectsOfType<PortalGimmicController>();
        m_gate.SetActive(false);
        m_gimmics[Random.Range(0, m_gimmics.Length)].IsGenuine = false;
    }

    public void CheckTask()
    {
        GameObject.FindObjectOfType<TimeManager>().DecreaseTime(m_decreaseTime);
        m_view.RPC(nameof(Check), RpcTarget.All);
    }

    [PunRPC]
    public void Check()
    {
        m_gate.SetActive(gimmic.Where(x => x.Complete).Count() + 1 == gimmic.Length);
    }
}
