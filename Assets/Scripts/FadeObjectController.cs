using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FadeObjectController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("フェードするスピード")]
    float m_speed = 1f;

    [SerializeField]
    [Tooltip("フェードする距離")]
    float m_fadeDistance = 3f;

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
        while (m_sr.material.color.a - alpha > 0.01f || m_sr.material.color.a - alpha < -0.01f)
        {
            m_setColor.a += (alpha - m_sr.material.color.a) / 100f * m_speed;
            m_sr.material.color = m_setColor;
            yield return null;
        }
        m_setColor.a = alpha;
        m_sr.material.color = m_setColor;

    }

    private void Update()
    {
        if(Camera.main.transform.position.z   > transform.position.z - m_fadeDistance)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoof(0.2f));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoof(1));
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if ((collision.CompareTag("Witch") || collision.CompareTag("Hunter")) && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoof(0.2f));
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if ((collision.CompareTag("Witch") || collision.CompareTag("Hunter")) && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoof(1));
        }
    }
}
