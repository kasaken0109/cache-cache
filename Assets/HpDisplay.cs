using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HpDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("HPを表示する画像")]
    private Image[] images = default;

    [SerializeField]
    [Tooltip("減るエフェクトのスピード")]
    private float m_speed = 1f;

    [SerializeField]
    [Tooltip("HPを表示するオブジェクト")]
    private GameObject m_display = null;


    PhotonView m_view = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitSet());
    }

    IEnumerator WaitSet()
    {
        m_display.SetActive(false);
        yield return new WaitUntil(() => GetComponent<PhotonView>());
        m_view = GetComponent<PhotonView>();
        yield return new WaitForSeconds(2f);
        m_display.SetActive(m_view.IsMine);
    }

    // Update is called once per frame
    public void UpdateHp(int hp)
    {
        StartCoroutine(UIEffect(images[hp]));
    }

    IEnumerator UIEffect(Image target)
    {
        while (target.fillAmount >= 0.01f)
        {
            target.fillAmount -= 0.01f * m_speed;
            yield return null;
        }
    }
}
