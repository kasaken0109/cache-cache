using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TrapController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Witch") && IsFirst)
        {
            PhotonView view = collision.gameObject.GetComponent<PhotonView>();

            if (view)
            {
                view.RPC("PlayStun", RpcTarget.All);
                IsFirst = false;
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    bool IsFirst = true;

    private void OnEnable()
    {
        IsFirst = true;
    }
}
