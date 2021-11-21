using UnityEngine;
using Photon.Pun;

// こっちはいじる必要なし、多分
public class Teleporter : MonoBehaviour
{
    int _myID;
    int _groupID;
    TeleportCreater _creater;

    public int MyID { set { _myID = value; } }
    public int GroupID { set { _groupID = value; } }
    public TeleportCreater Creater { set { _creater = value; } }

    bool _isStay = false;
    GameObject _target = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Witch")
            && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            _target = collision.gameObject;
            _isStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Witch")
            && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            _target = null;
            _isStay = false;
        }
    }

    void Update()
    {
        if (_isStay && Input.GetKeyDown(KeyCode.Space))
            _creater.TPRequest(_groupID, _myID, _target);
    }
}
