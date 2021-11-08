using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedVisibility : Item
{
    [SerializeField]
    [Tooltip("効果時間")]
    private int m_time = 20;

    private HunterCamera m_hunterCamera = default;

    // Start is called before the first frame update
    void Start()
    {
        m_hunterCamera = GetComponentInChildren<HunterCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseItem()
    {
        StartCoroutine(nameof(Use));
    }

    IEnumerator Use()
    {
        StartCoroutine(m_hunterCamera.FoVChange(m_time));
        yield return new WaitForSeconds(m_time + 2);
        base.UseItem();
    }
}
