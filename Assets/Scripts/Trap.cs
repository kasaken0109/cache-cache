using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Trap : Item
{
    [SerializeField]
    [Tooltip("設置時間")]
    float m_trapTime = 1f;
    string m_trap = "Trap";
    Hunter m_hunter = default;
    // Start is called before the first frame update
    void Start()
    {
        m_hunter = GameObject.FindGameObjectWithTag("Hunter").GetComponent<Hunter>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Jump")) UseItem();
    }
    GameObject[] traps;
    public override void UseItem()
    {
        traps = GameObject.FindGameObjectsWithTag("Trap");
        StartCoroutine(nameof(WaitCharge));
        GameObject trap = PhotonNetwork.Instantiate(m_trap, m_hunter.transform.position, m_hunter.transform.rotation);
        if (traps.Length > 2)
        {
            PhotonNetwork.Destroy(GameObject.Find("trap1"));
            traps = GameObject.FindGameObjectsWithTag("Trap");
            for (int i = 0; i < traps.Length - 1; i++)
            {
                traps[i] = GameObject.Find("trap" + (i + 1)) ? GameObject.Find("trap" + (i + 1)) : GameObject.Find("trap" + (i + 2));
                traps[i].name = "trap" + (i + 1).ToString();
            }
            trap.name = "trap" + traps.Length.ToString();
        }
        else
        {
            trap.name = "trap" + traps == null ? "trap" + "1" : "trap" + (traps.Length + 1).ToString();
        }
        base.UseItem();
    }

    IEnumerator WaitCharge()
    {
        float timer = 0;
        while (timer < m_trapTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
