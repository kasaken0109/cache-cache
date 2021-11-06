using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Item
{
    [SerializeField]
    [Tooltip("速度補正値")]
    private float m_speedUp = 1.8f;

    [SerializeField]
    [Tooltip("効果時間")]
    private float m_time = 20f;

    private　Hunter m_hunter = default;

    // Start is called before the first frame update
    void Start()
    {
        m_hunter = GetComponent<Hunter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseItem()
    {
        StopCoroutine(nameof(UpSpeed));
        StartCoroutine(nameof(UpSpeed));
        base.UseItem();
    }

    IEnumerator UpSpeed()
    {
        m_hunter.SpeedUpRate = m_speedUp;
        yield return new WaitForSeconds(m_time);
        m_hunter.SpeedUpRate = 1f;
    }
}
