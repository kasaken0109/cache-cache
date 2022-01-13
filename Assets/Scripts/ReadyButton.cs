using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ReadyButton : MonoBehaviour
{
    [SerializeField]
    GameObject m_button, m_image;
    public bool ReadyCheck { get; private set; }
    PhotonView m_view;
    NetworkGameManager m_nW;
    GameManager m_gM;
    private void Start()
    {
        m_nW = FindObjectOfType<NetworkGameManager>();
        m_gM = FindObjectOfType<GameManager>();
        StartCoroutine(GetNW());
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    IEnumerator GetNW()
    {
        while (!m_nW.ReadyFlag)
        {
            yield return null;
            Debug.Log("まだ");
        }
        yield return new WaitForEndOfFrame();
        Active();
    }
    public void OnReady()
    {
        if (!m_view)
        {
            m_view = GameObject.Find("JudgementController").GetComponent<PhotonView>();
        }
        if (!ReadyCheck)
        {
            m_view.RPC("Check", RpcTarget.All);
            ReadyCheck = true;
        }
        else
        {
            m_view.RPC("UncChecked", RpcTarget.All);
            ReadyCheck = false;
        }
    }
    void Active()
    {
        if (m_nW.ReadyFlag)
        {
            m_button.SetActive(true);
            m_nW.ReadyFlag = false;
        }
        else
        {
            m_button.SetActive(false);
        }
    }
    public void CheckActive()
    {
        if (ReadyCheck)
        {
            m_image.SetActive(true);
        }
        else
        {
            m_image.SetActive(false);
        }
    }
}
