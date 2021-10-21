using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Animal")) GetComponentInParent<Hunter>().PlayStun();
        else if (collision.CompareTag("Witch")) collision.GetComponent<Witch>().OnHit();
    }
}
