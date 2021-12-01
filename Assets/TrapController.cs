using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TrapController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Witch") && IsFirst)
        {
            PhotonView view = other.gameObject.GetComponent<PhotonView>();

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
