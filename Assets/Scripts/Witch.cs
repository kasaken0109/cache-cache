using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : CharaBase
{
    Rigidbody2D m_rb;

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
}
