using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AlartSet : Item
{
    [SerializeField]
    [Tooltip("設置時間")]
    float m_setTime = 1f;
    string m_alart = "AlartPrefab";
    Hunter m_hunter = default;
    // Start is called before the first frame update
    void Start()
    {
        m_hunter = GameObject.FindGameObjectWithTag("Hunter").GetComponent<Hunter>();
    }

    public override void UseItem()
    {
        StartCoroutine(nameof(WaitCharge));
        GameObject trap = PhotonNetwork.Instantiate(m_alart, m_hunter.transform.position, m_hunter.transform.rotation);
        base.UseItem();
    }

    IEnumerator WaitCharge()
    {
        float timer = 0;
        while (timer < m_setTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
