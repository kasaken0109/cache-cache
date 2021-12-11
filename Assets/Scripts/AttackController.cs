using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackController : MonoBehaviour
{
    [SerializeField] bool m_isHunter = true;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Animal") && m_isHunter) GetComponentInParent<Hunter>().PlayStun();
        if (collision.CompareTag("Rock") && !m_isHunter && IsFirst)
        {
            collision.GetComponent<PortalGimmicController>().Damage();
            IsFirst = false;
        }
        else if (collision.CompareTag("Hunter") && !m_isHunter) collision.GetComponentInParent<Hunter>().PlayStun();
        else if (collision.CompareTag("Witch") && IsFirst)
        {
            //collision.GetComponent<Witch>().OnHit();
            PhotonView view = collision.gameObject.GetComponent<PhotonView>();

            if (view)
            {
                view.RPC("OnHit", RpcTarget.All);
            }
            Debug.Log("HunterAttack");
            IsFirst = false;
        }
    }
    bool IsFirst = true;

    private void OnEnable()
    {
        IsFirst = true;
    }
}
