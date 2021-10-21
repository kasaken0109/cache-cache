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

    void Start()
    {
        m_view = GetComponent<PhotonView>();
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = 0;
    }
    private void FixedUpdate()
    {
        if (!m_view || !m_view.IsMine) return;      // 自分が生成したものだけ処理する
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
    }

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector2(h, v).normalized * Speed;
    }

    public void OnHit()
    {
        m_hp--; //1ずつ減らす

        if (m_hp < 1) //魔法使いの体力が1未満になったら呼び出す
        {
            Debug.Log("HPが0になった。");
        }
    }
}
