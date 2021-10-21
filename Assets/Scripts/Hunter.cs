using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    [Tooltip("向きの点")]
    Transform[] m_directionPoints = default;

    [SerializeField]
    [Tooltip("攻撃判定が発生する時間")]
    float m_attackTime = 0.2f;
    bool CanMove = true;
    PhotonView m_view;

    void Start()
    {
        m_view = GetComponent<PhotonView>();
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = 0;
    }
    private void FixedUpdate()
    {
        if (!m_view || !m_view.IsMine) return;      // 自分が生成したものだけ処理する
        if (CanMove)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Move(h, v);
            SetDirection(h, v);
        }
        if (Input.GetButtonDown("Fire1")) StartCoroutine(nameof(Attack));
    }

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector2(h, v).normalized * Speed;
    }
    int direction = 2;
    public int SetDirection(float h, float v)
    {
        if (h == 0 && v == 0) return direction;
        else if (h < 0 && v == 0) direction = 1;
        else if (h > 0 && v == 0) direction = 2;
        else if (h == 0 && v > 0) direction = 3;
        else if (h == 0 && v < 0) direction = 4;
        m_attackObject.transform.SetParent(m_directionPoints[direction - 1]);
        m_attackObject.transform.localPosition = Vector3.zero;
        return direction;
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
}
