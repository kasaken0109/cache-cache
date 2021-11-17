using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FadeRoofController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("フェードするスピード")]
    float m_speed = 1f;

    SpriteRenderer m_sr = default;
    Color m_setColor;
    // Start is called before the first frame update
    void Start()
    {
        m_sr = GetComponent<SpriteRenderer>();
        m_setColor = m_sr.color;
    }

    IEnumerator FadeRoof(float alpha)
    {
        while (m_sr.color.a - alpha > 0.01f || m_sr.color.a - alpha < -0.01f)
        {
            m_setColor.a += (alpha - m_sr.color.a) / 100f * m_speed;
            m_sr.color = m_setColor;
            yield return null;
        }
        m_setColor.a = alpha;
        m_sr.color = m_setColor;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Witch") || collision.CompareTag("Hunter")) && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoof(0));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Witch") || collision.CompareTag("Hunter")) && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoof(1));
        }
    }
}
