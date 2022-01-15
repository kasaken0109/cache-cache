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
        if (collision.CompareTag("Animal") && m_isHunter && !m_isLight)
        {
            GetComponentInParent<Hunter>().PlayStun(false);
            string name = collision.gameObject.name;
            if (collision.gameObject.name.Contains("くま")) Sounds.SoundMaster.Request(collision.gameObject.transform, 2, 1);
            else if (collision.gameObject.name.Contains("きつね")) Sounds.SoundMaster.Request(collision.gameObject.transform, 7, 1);
            else if (collision.gameObject.name.Contains("うし")) Sounds.SoundMaster.Request(collision.gameObject.transform, 2, 1);
            else if (collision.gameObject.name.Contains("とり")) Sounds.SoundMaster.Request(collision.gameObject.transform, 3, 1);
        }
        if (collision.CompareTag("Rock") && !m_isHunter && IsFirst && !m_isLight)
        {
            collision.GetComponent<PortalGimmicController>().Damage();
            ScoreManager.RequestAddScore(ActionScore.AttackStone);
            IsFirst = false;
        }
        else if (collision.CompareTag("Hunter") && !m_isHunter && m_isLight)
        {
            collision.GetComponentInParent<Hunter>().PlayStun(true);
            ScoreManager.RequestAddScore(ActionScore.HitLight);
        }
        else if (collision.CompareTag("Witch") && IsFirst && !m_isLight)
        {
            PhotonView view = collision.gameObject.GetComponent<PhotonView>();

            if (view)
            {
                view.RPC("OnHit", RpcTarget.All);
            }
            ScoreManager.RequestAddScore(ActionScore.AttackHit);
            IsFirst = false;
        }
    }
    bool IsFirst = true;

    private void OnEnable()
    {
        IsFirst = true;
    }
}
