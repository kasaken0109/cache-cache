using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIndicator : MonoBehaviour
{
    SpriteRenderer m_sr;
    private void Start()
    {
        m_sr = GetComponent<SpriteRenderer>();
    }

    public void SetEnable(bool value)
    {
        m_sr.enabled = value;
    }
}
