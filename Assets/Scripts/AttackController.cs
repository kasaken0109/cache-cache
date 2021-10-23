using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Animal")) GetComponentInParent<Hunter>().PlayStun();
        else if (collision.CompareTag("Witch"))
        {
            collision.GetComponent<Witch>().OnHit();
            Debug.Log("HunterAttack");
            IsFirst = false;
        }
    }
    bool IsFirst = true;
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Animal") && IsFirst)
    //    {
    //        GetComponentInParent<Hunter>().PlayStun();
    //        IsFirst = false;
    //    }
    //    else if (collision.CompareTag("Witch") && IsFirst)
    //    {
    //        collision.GetComponent<Witch>().OnHit();
    //        Debug.Log("HunterAttack");
    //        IsFirst = false;
    //    }
    //}

    private void OnEnable()
    {
        IsFirst = true;
    }
}
