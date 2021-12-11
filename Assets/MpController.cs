using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("MP回復量")]
    float m_cureMp = 30f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Witch"))
        {
            other.GetComponent<Witch>().SetMp(m_cureMp);
            Destroy(gameObject, 1f);
        }
    }
}
