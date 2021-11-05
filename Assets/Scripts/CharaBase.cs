using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class CharaBase : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] string m_name;
    public bool CanMove = true;

    public float Speed { get => m_speed; private set { m_speed = value; } }
    public string Name { get => m_name; set { m_name = value; } }
    public abstract void Move(float h, float v);
}
