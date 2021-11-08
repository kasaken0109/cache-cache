using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("表示するアイテム画像")]
    Sprite[] m_itemImages = default;

    Image m_image = default;
    private void Start()
    {
        m_image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    public void ChangeItem(int type)
    {
        m_image.sprite = m_itemImages[type];
    }
}
