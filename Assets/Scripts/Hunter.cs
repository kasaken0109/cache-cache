using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : CharaBase
{
    Rigidbody2D m_rb;

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
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = 0;
    }

    void Update()
    {
        if (CanMove)
        {
            float h = Input.GetAxisRaw("Horizontal2");
            float v = Input.GetAxisRaw("Vertical2");
            Move(h, v);
        }
        if (Input.GetButtonDown("Fire1")) StartCoroutine(nameof(Attack));
    }

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector2(h, v) * Speed;
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
