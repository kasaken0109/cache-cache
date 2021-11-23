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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Witch")
            && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            m_target = collision.gameObject;
            m_isStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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
        if (m_isStay && Input.GetKeyUp(KeyCode.Space) && transform.position.z == m_target.transform.position.z)
        {
            Debug.Log("押した");
            Request();
        }
    }

    void Request()
    {
        Debug.Log($"MyCol {transform.position.z} Target {m_target.transform.position.z}");
        m_creater.TPRequest(m_groupID, m_myID, m_target);
    }
}
