using UnityEngine;
using Photon.Pun;

// こっちはいじる必要なし、多分
public class Teleporter : MonoBehaviour
{
    int m_myID;
    int m_groupID;
    TeleportManager m_creater;

    public int MyID { set { m_myID = value; } }
    public int GroupID { set { m_groupID = value; } }
    public TeleportManager Creater { set { m_creater = value; } }

    bool m_isStay = false;
    GameObject m_target = null;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Witch")
            && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            m_target = collision.gameObject;
            m_isStay = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Witch")
            && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            m_target = null;
            m_isStay = false;
        }
    }

    void Update()
    {
        if (m_isStay && Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("押した");
            Debug.Log($"MYID {m_myID} GroupID {m_groupID}");
            Request();
        }
    }

    void Request() => m_creater.TPRequest(m_groupID, m_myID, m_target);
}
