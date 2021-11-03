using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [SerializeField]
    [Tooltip("別のアイテムが使えるようになる時間")]
    private float m_cooldown = 5f;

    /// <summary>
    /// 別のアイテムが使えるようになる時間
    /// </summary>
    public float CoolDown => m_cooldown;
    // Start is called before the first frame update

    public virtual void UseItem()
    {
        //アイテムごとの処理の関数
        Destroy(this);
    }
}
