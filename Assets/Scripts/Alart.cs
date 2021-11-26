using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Alart : MonoBehaviour
{
    private string m_setLayerName = null;
    public string SetLayerName => m_setLayerName;
    // Start is called before the first frame update
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Witch") && IsFirst)
        {
            PhotonView view = collision.gameObject.GetComponent<PhotonView>();

            if (view)
            {

                Debug.Log("HitWitch");
                var w = collision.GetComponent<PositionIndicator>();
                if(GameObject.FindWithTag("Hunter").GetComponent<PhotonView>().IsMine) LayerManager.Instance.SetWitchPos(w.MyLayerNum.ToString());
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
