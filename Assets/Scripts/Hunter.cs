﻿using System.Collections;
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
    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) StartCoroutine(nameof(Attack));
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
        //if (Input.GetButtonDown("Fire1")) StartCoroutine(nameof(Attack));
    }

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector2(h, v).normalized * Speed;
    }

    public void SetDirection(float h, float v)
    {
        m_attackObject.transform.localPosition = new Vector3(h * 1.5f,v * 1.5f,0);
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
