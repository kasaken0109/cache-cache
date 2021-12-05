using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("表示するアイテム画像")]
    Sprite[] m_itemImages = default;

    [SerializeField]
    [Tooltip("表示するアイテムオブジェクト")]
    GameObject m_itemdisplay = default;

    [SerializeField]
    [Tooltip("imagecomponent")]
    Image m_image = default;

    PhotonView m_view = null;
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitSet());
    }

    IEnumerator WaitSet()
    {
        m_itemdisplay.SetActive(false);
        yield return new WaitUntil(() => GetComponent<PhotonView>());
        m_view = GetComponent<PhotonView>();
        yield return new WaitForSeconds(2f);
        m_itemdisplay.SetActive(m_view.IsMine);
    }
    // Start is called before the first frame update
    public void ChangeItem(int type)
    {
        m_image.sprite = m_itemImages[type];
    }
}
