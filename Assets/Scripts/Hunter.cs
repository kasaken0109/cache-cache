using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : CharaBase
{
    [SerializeField]
    [Tooltip("スタンする時間")]
    float m_stunTime = 3f;

    [SerializeField]
    [Tooltip("攻撃のコライダー")]
    GameObject m_attackObject = default;

    [SerializeField]
    [Tooltip("攻撃判定が発生する時間")]
    float m_attackTime = 0.5f;
    bool CanMove = true;
    void Start()
    {
        
    }

    void Update()
    {
        if(CanMove)base.Move();
        if (Input.GetButtonDown("Fire1")) StartCoroutine(nameof(Attack));
    }

    IEnumerator Attack()
    {
        m_attackObject.SetActive(true);
        yield return new WaitForSeconds(m_attackTime);
        m_attackObject.SetActive(false);
    }

    public void PlayStun()
    {
        StartCoroutine(nameof(Stun));
    }

    IEnumerator Stun()
    {
        CanMove = false;
        yield return new WaitForSeconds(m_stunTime);
        CanMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Animal")) StartCoroutine(nameof(Stun));
        //else if (collision.CompareTag("Witch")) collision.GetComponent<Witch>().OnHit();
    }

}
