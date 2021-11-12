﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : CharaBase
{
    [Header("アタリ判定用子オブジェクト")] [SerializeField]
    Collider2D m_trigger = default;
    [Header("最大移動時間")] [SerializeField]
    float m_maxActionTime = 3f;
    [Header("最小移動時間")] [SerializeField]
    float m_minActionTime = 1f;
    [Header("最大待機時間")] [SerializeField]
    float m_maxWaitTime = 5f;
    [Header("最小待機時間")] [SerializeField]
    float m_minWaitTime = 2f;

    Rigidbody2D m_rb = default;
    Animator m_anim = default;

    float m_timer = 0f;
    float m_actionTimer = 0f;
    float m_waitTime = 0f;

    Vector2 m_dist = default;
    bool m_action = false;
    public void CreateStart()
    {
        m_trigger.gameObject.SetActive(false);
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
    }


    /// <summary>
    /// 移動、行動AI
    /// </summary>
    public void MovingUpdate()
    {
        /// 行動中は動かない
        if (m_action) 
        {
            if (m_timer < m_actionTimer)
            {
                m_timer += Time.deltaTime;
            }
            else
            {

                m_timer = 0;
                m_action = false;
                m_rb.velocity = Vector2.zero;
            }

            return;
        }

        if (m_timer < m_waitTime)
        {
            m_timer += Time.deltaTime;
            return;
        }
        else
        {
            m_timer = 0;
            m_action = true;
            m_waitTime = UnityEngine.Random.Range(m_minWaitTime, m_maxWaitTime);
            m_actionTimer = UnityEngine.Random.Range(m_minActionTime, m_maxActionTime);

            Run();
        }
    }

    /// <summary>
    /// 走る
    /// </summary>
    void Run()
    {
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));
        m_dist = direction.normalized;
        m_trigger.transform.localPosition = m_dist;
        m_trigger.gameObject.SetActive(true);
        Move(direction.x, direction.y);
    }

    /// <summary>
    /// 移動する
    /// </summary>
    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector2(h, v) * Speed;
        m_anim.SetBool("IsWalk", true);
        
    }


    /// 何かに当たると移動を止める
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_trigger.gameObject.SetActive(false);
        m_timer = 0;
        m_action = false;
        m_rb.velocity = Vector2.zero;
        m_anim.SetBool("IsWalk", false);
    }
}
