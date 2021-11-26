using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LayerDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("階層を表示するUI")]
    private Text m_display = default;

    [SerializeField]
    [Tooltip("デバッグモード")]
    private bool m_debug = false;

    [SerializeField]
    [Tooltip("レイヤー番号")]
    private int m_layerNum = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Hunter") || collision.CompareTag("Witch")) 
            && (m_debug || !collision.GetComponent<PhotonView>().IsMine) && collision.transform.position.z == transform.position.z)
        {
            collision.GetComponent<PositionIndicator>().MyLayerNum = m_layerNum;
            m_display.text = "Layer" + m_layerNum;
        }
    }

    bool IsFirst = true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Hunter") || collision.CompareTag("Witch")) 
            && (m_debug || collision.GetComponent<PhotonView>().IsMine) && collision.transform.position.z == transform.position.z && IsFirst)
        {
            Debug.Log("Hit");
            collision.GetComponent<PositionIndicator>().MyLayerNum = m_layerNum;
            m_display.text = "Layer" + m_layerNum;
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
