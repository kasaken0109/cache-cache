using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Witch : CharaBase
{
    Rigidbody2D m_rb;
    PhotonView m_view;
    [SerializeField]
    private int m_hp = 3; //魔法使いの体力

    [SerializeField]
    [Tooltip("変身範囲のコライダー")]
    GameObject m_cacheRangeObject = default;
    [SerializeField]
    [Tooltip("向きの点")]
    Transform[] m_directionPoints = default;
    [SerializeField]
    [Tooltip("生成するアラート")]
    private GameObject m_alart = default;

    public int Hp => m_hp;
    HpDisplay hpDisplay = default;
    Collider2D m_change;
    SpriteRenderer m_sr;
    bool m_contactFlag = false;
    void Start()
    {
        m_sr = GetComponent<SpriteRenderer>();
        m_change = GetComponentInChildren<Collider2D>();
        m_view = GetComponent<PhotonView>();
        m_rb = GetComponent<Rigidbody2D>();
        hpDisplay = GetComponent<HpDisplay>();
        m_rb.gravityScale = 0;
    }
    private void Update()
    {
        ChangeSprite();
    }
    private void FixedUpdate()
    {
        if (!m_view || !m_view.IsMine) return;      // 自分が生成したものだけ処理する
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        SetDirection(h, v);
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
        m_cacheRangeObject.transform.SetParent(m_directionPoints[direction - 1]);
        m_cacheRangeObject.transform.localPosition = Vector3.zero;
        return direction;
    }

    public void OnHit()
    {
        m_hp--; //1ずつ減らす

        if (m_hp < 1) //魔法使いの体力が1未満になったら呼び出す
        {
            m_hp = 0;
            Debug.Log("HPが0になった。");
        }
        hpDisplay.UpdateHp(m_hp);
    }
    void ChangeSprite()
    {
        if (m_contactFlag)
        {
            if (Input.GetButtonDown("Use"))
            {
                Debug.Log("押された");
                if (m_change.gameObject.CompareTag("Animal"))
                {
                    m_sr.sprite = m_change.gameObject.GetComponent<SpriteRenderer>().sprite;
                    Debug.Log(m_sr.sprite);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        m_change = other;
        m_contactFlag = true;
        Debug.Log("入った");
    }
    private void OnTriggerExit2D()//Collider2D other)
    {
        m_change = null;
        m_contactFlag = false;
        Debug.Log("でた");
    }

    public void SpawnAlarm()
    {
        Instantiate(m_alart, transform.position, transform.rotation);
    }
}
