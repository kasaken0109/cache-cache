using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : CharaBase
{
    Rigidbody2D m_rb;

    [SerializeField]
    private int m_hp = 3; //魔法使いの体力

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = 0;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal1");
        float v = Input.GetAxisRaw("Vertical1");

        Move(h, v);
    }

    public override void Move(float h, float v)
    {
        m_rb.velocity = new Vector2(h, v) * Speed;
    }

    public void OnHit()
    {
        m_hp--; //1ずつ減らす

        if (m_hp < 1) //魔法使いの体力が1以下になったら呼び出す
        {
            Debug.Log("HPが0になった。");
        }
    }
}
