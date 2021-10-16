using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharaBase : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] bool m_isWitch;

    Rigidbody2D RB { get; set; }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.gravityScale = 0;
    }

    public virtual void Move()
    {
        float h = 0;
        float v = 0;

        if (m_isWitch)
        {
            h = Input.GetAxisRaw("Horizontal1");
            v = Input.GetAxisRaw("Vertical1");
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal2");
            v = Input.GetAxisRaw("Vertical2");
        }

        RB.velocity = new Vector2(h, v) * m_speed;
    }
}
