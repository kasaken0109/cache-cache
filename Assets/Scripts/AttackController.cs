using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackController : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Animal")) GetComponentInParent<Hunter>().PlayStun();
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
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Animal") && IsFirst)
    //    {
    //        GetComponentInParent<Hunter>().PlayStun();
    //        IsFirst = false;
    //    }
    //    else if (collision.CompareTag("Witch") && IsFirst)
    //    {
    //        collision.GetComponent<Witch>().OnHit();
    //        Debug.Log("HunterAttack");
    //        IsFirst = false;
    //    }
    //}

    private void OnEnable()
    {
        IsFirst = true;
    }
}
