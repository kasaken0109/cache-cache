using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class MiniMapController : MonoBehaviour
{
    CharaBase[] m_charas;
    CharaBase m_playChara;
    bool IsWitch;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(SetUp));
    }

    private IEnumerator SetUp()
    {
        yield return new WaitForSeconds(5f);
        m_charas = GameObject.FindObjectsOfType<CharaBase>();
        m_playChara = m_charas.Single(c => (c.gameObject.tag == "Witch" || c.gameObject.tag == "Hunter") && c.gameObject.GetComponent<PhotonView>().IsMine);
        IsWitch = m_playChara as Witch;
        if (IsWitch)
        {
            GameObject.FindGameObjectWithTag("Hunter").GetComponentInChildren<MinimapIndicator>().SetEnable(false);
        }
        else
        {
            var witch = GameObject.FindGameObjectsWithTag("Witch");
            foreach (var item in witch)
            {
                item.GetComponentInChildren<MinimapIndicator>().SetEnable(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_playChara) return;
        transform.position = new Vector3(m_playChara.transform.position.x, transform.position.y, m_playChara.transform.position.z);
    }
}
