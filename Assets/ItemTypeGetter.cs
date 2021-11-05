using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTypeGetter : MonoBehaviour
{
    [SerializeField]
    Hunter.HaveItemType itemType = Hunter.HaveItemType.None;

    public Hunter.HaveItemType ItemType => itemType;
    
}
