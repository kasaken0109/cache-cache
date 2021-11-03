using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Trap : Item
{
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
        if (Input.GetButtonDown("Jump")) UseItem();
    }

    public override void UseItem()
    {
        PhotonNetwork.Instantiate(m_trap,m_hunter.transform.position,m_hunter.transform.rotation);
        base.UseItem();
    }
}
