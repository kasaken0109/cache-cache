using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMove : MonoBehaviour
{
    [Header("アタリ判定用子オブジェクト")] [SerializeField]
    Collider2D m_trigger = default;
    [Header("移動スピード")] [SerializeField] 
    float m_speed = default;
    [Header("最大移動時間")] [SerializeField]
    float m_maxActionTime = default;
    [Header("最小移動時間")] [SerializeField]
    float m_minActionTime = default;
    [Header("最大待機時間")] [SerializeField]
    float m_maxWaitTime = 3f;
    [Header("最小待機時間")] [SerializeField]
    float m_minWaitTime = 1f;

    Rigidbody2D m_rb = default;

    float m_timer = 0f;
    float m_actionTimer = 0f;
    float m_waitTime = 0f;

    Vector2 m_dist = default;
    bool m_action = false;
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// あとで消す
    /// </summary>
    private void Update()
    {
        MovingUpdate();
    }

    /// <summary>
    /// 移動、行動AI
    /// </summary>
    private void MovingUpdate()
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
        AnimalMoving(direction);
    }

    /// <summary>
    /// 移動する
    /// </summary>
    /// <param name="direction"></param>
    private void AnimalMoving(Vector2 direction)
    {
        m_rb.velocity = direction * m_speed;
    }


    /// 何かに当たると移動を止める
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_timer = 0;
        m_action = false;
        m_rb.velocity = Vector2.zero;
    }
}
