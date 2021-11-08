using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTypeGetter : MonoBehaviour
{
    [SerializeField]
    Hunter.HaveItemType itemType = Hunter.HaveItemType.None;

    public Hunter.HaveItemType ItemType => itemType;

    private int m_id = 0;

    public int ID { get => m_id; set { m_id = value; } }
    
}
