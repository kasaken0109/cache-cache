using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    bool SelfSupport = false;

    [Header("最大移動時間")] [SerializeField]
    float m_maxActionTime = 3f;
    [Header("最小移動時間")] [SerializeField]
    float m_minActionTime = 1f;
    [Header("最大待機時間")] [SerializeField]
    float m_maxWaitTime = 5f;
    [Header("最小待機時間")] [SerializeField]
    float m_minWaitTime = 2f;

    Rigidbody m_rb = default;
    Animator m_anim = default;
    //Animator m_shadowAnim = default;

    [SerializeField]
    float m_speed = 0f;
    float m_timer = 0f;
    float m_actionTimer = 0f;
    float m_waitTime = 0f;
    Vector2 m_halfSize = default;

    RaycastHit hit = default;
    Vector3Int m_direction = new Vector3Int();
    bool m_action = false;
    int m_mask = 1 << 13;

    public void Start()
    {
        //m_trigger.gameObject.SetActive(false);
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_halfSize = GetComponent<BoxCollider>().size / 2;
        //m_shadowAnim = transform.GetChild(0).GetComponent<Animator>();

        if (SelfSupport)
        {
            StartCoroutine(Moving());
        }
    }
    IEnumerator Moving()
    {
        while (true)
        {
            MovingUpdate();
            yield return null;
        }
    }

    /// <summary>
    /// 移動、行動AI
    /// </summary>
    public void MovingUpdate()
    {
        if (m_action) 
        {
            if (m_timer < m_actionTimer)
            {
                m_timer += Time.deltaTime;

                // rayを飛ばして足元を確認
                //int direction = 0; 
                //if (transform.rotation.y == 0) direction = 1;
                //else if (transform.rotation.y == 1) direction = - 1;

                Ray ray = GroundCheck();

                if (!Physics.Raycast(ray, out hit, 0.5f, m_mask))
                {
                    MoveStop();
                }
            }
            else
            {
                MoveStop();
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
        m_direction.x = UnityEngine.Random.Range(-1, 2);
        m_direction.z = UnityEngine.Random.Range(-1, 2);

        Ray ray = GroundCheck();

        if ((m_direction.x == 0 && m_direction.z == 0) || !Physics.Raycast(ray, out hit, 0.5f, m_mask)) return; 
        else if (m_direction.x >= 1) transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (m_direction.x >= -1) transform.rotation = Quaternion.Euler(0, 180, 0);

        Move(m_direction.x, m_direction.z);
    }

    /// <summary>
    /// 移動する
    /// </summary>
    public void Move(float h, float z)
    {
        m_rb.velocity = new Vector3(h, 0, z).normalized * m_speed;
        m_rb.constraints = m_rb.velocity == Vector3.zero ? RigidbodyConstraints.FreezePosition 
            | RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeRotation;
        m_anim.SetBool("IsWalk", true);
    }

    private void MoveStop()
    {
        m_timer = 0;
        m_action = false;
        m_anim.SetBool("IsWalk", false);
        //m_shadowAnim.SetBool("IsWalk", false);
        m_rb.velocity = Vector2.zero;
    }


    /// 何かに当たると移動を止める
    private void OnTriggerEnter(Collider collision)
    {
        if (m_action)
        {
            MoveStop();
        }
    }

    Ray GroundCheck()
    {
        Vector3 rayPosition = gameObject.transform.position + new Vector3((m_halfSize.x + 0.001f) * m_direction.x, -m_halfSize.y - 0.001f, 0.5f + m_direction.z);
        return new Ray(rayPosition, new Vector3(m_direction.x, -1, m_direction.z));
        //Debug.DrawRay(rayPosition, ray.direction * 0.5f, Color.black);
    }
}
