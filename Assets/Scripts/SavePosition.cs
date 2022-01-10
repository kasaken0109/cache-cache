using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class SavePosition : MonoBehaviour
{
    Action<Vector3, GameObject> _action;

    public void SetUp(Action<Vector3, GameObject> action)
    {
        _action = action;
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView view = other.GetComponent<PhotonView>();
        GameObject obj = other.gameObject;
        if (view != null && view.IsMine && obj.CompareTag("Witch"))
        {
            _action.Invoke(transform.position, obj);
        }
    }
}
