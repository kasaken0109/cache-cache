using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackController : MonoBehaviour
{
    [SerializeField] bool m_isHunter = true;
    [SerializeField] bool m_isLight = false;
    [SerializeField] float m_stunTime = 0;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Animal") && m_isHunter && !m_isLight) GetComponentInParent<Hunter>().PlayStun(false);
        if (collision.CompareTag("Rock") && !m_isHunter && IsFirst && !m_isLight)
        {
            collision.GetComponent<PortalGimmicController>().Damage();
            IsFirst = false;
        }
        else if (collision.CompareTag("Hunter") && !m_isHunter && m_isLight) collision.GetComponentInParent<Hunter>().PlayStun(true);
        else if (collision.CompareTag("Witch") && IsFirst && !m_isLight)
        {
            PhotonView view = collision.gameObject.GetComponent<PhotonView>();

            if (view)
            {
                view.RPC("OnHit", RpcTarget.All);
            }
            IsFirst = false;
        }
    }
    bool IsFirst = true;

    private void OnEnable()
    {
        IsFirst = true;
    }
}
