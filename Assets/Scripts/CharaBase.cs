using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class CharaBase : MonoBehaviour
{
    [SerializeField] float m_speed;

    public float Speed { get => m_speed; private set { m_speed = value; } }
    public abstract void Move(float h, float v);
}
