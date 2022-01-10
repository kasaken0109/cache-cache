using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PositionSever : MonoBehaviour
{
    LayerMask _layer;
    Vector3 _savePos = Vector3.zero;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.AddComponent<SavePosition>().SetUp(PostionUpDate);
        }
    }

    void PostionUpDate(Vector3 savePos, GameObject obj)
    {
        _savePos = savePos;
        _layer = obj.layer;
    }

    private void OnCollisionStay(Collision collision)
    {
        PhotonView view = collision.gameObject.GetComponent<PhotonView>();
        GameObject obj = collision.gameObject;
        float posY = obj.transform.position.y;
        if (view != null && view.IsMine && obj.CompareTag("Witch") && posY > 3)
        {
            obj.transform.position = _savePos;
        }
    }
}
