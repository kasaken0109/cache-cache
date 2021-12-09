using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class PortalGimmicManager : MonoBehaviour
{
    public static PortalGimmicManager Instance { get; private set; } 

    [SerializeField]
    [Tooltip("発生させる出口")]
    GameObject m_gate = default;

    PortalGimmicController[] gimmic;

    PhotonView m_view;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gimmic = GameObject.FindObjectsOfType<PortalGimmicController>();
        m_view = GetComponent<PhotonView>();
        m_gate.SetActive(false);
    }

    public void CheckTask()
    {
        m_view.RPC("Check",RpcTarget.All);
    }

    [PunRPC]
    public void Check()
    {
        m_gate.SetActive(gimmic.All(x => x.Complete));
    }
}
