using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffRoll_mikane : MonoBehaviour
{
    [SerializeField]
    Text m_staffRoll;       //スタッフロールのテキスト
    public GameObject m_staffRollManager;      //スタッフロールマネージャー

    public float spped = 1;     //流れるスピード
    public float positionY = 2000;      //止まる位置

    void Update()
    {
        StaffRoll();
    }

    void StaffRoll()
    {
        m_staffRoll.transform.Translate(0, spped, 0);       //スタッフロールを流す

        if (m_staffRoll.rectTransform.position.y >= positionY)
        {
            m_staffRollManager.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Space))        //早送り
        {
            spped = 20;
        }
        else
        {
            spped = 1;
        }
    }
}