using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LayerDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("階層を表示するUI")]
    Text m_display = default;

    [SerializeField]
    [Tooltip("デバッグモード")]
    bool m_debug = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Hunter") || collision.CompareTag("Witch")) 
            && (m_debug || collision.GetComponent<PhotonView>().IsMine) && collision.transform.position.z == transform.position.z)
        {
            Debug.Log("Enter");
            m_display.text = gameObject.name;
        }
    }

    bool IsFirst = true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Hunter") || collision.CompareTag("Witch")) 
            && (m_debug || collision.GetComponent<PhotonView>().IsMine) && collision.transform.position.z == transform.position.z && IsFirst)
        {
            m_display.text = gameObject.name;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Hunter") || collision.CompareTag("Witch")) 
            && (m_debug || collision.GetComponent<PhotonView>().IsMine))
        {
            IsFirst = false;
        }
    }
}
