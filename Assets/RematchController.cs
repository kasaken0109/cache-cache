using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class RematchController : MonoBehaviour
{
    [SerializeField]
    private SceneLoader m_sceneLoader = default;
    [SerializeField]
    private GameObject[] m_display = default;
    public void StartRematch()
    {
        StartCoroutine(m_sceneLoader.LoadScene(0));
        Rematch();
    }
    void Rematch()
    {
        var net = FindObjectOfType<NetworkGameManager>();
        net.Connect("1.0");
        PhotonNetwork.AutomaticallySyncScene = false;
        RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
        raiseEventoptions.Receivers = ReceiverGroup.All;
        SendOptions sendOptions = new SendOptions();
        PhotonNetwork.RaiseEvent((byte)NetworkEvents.Lobby, null, raiseEventoptions, sendOptions);
    }
    bool IsActive = true;
    public void SetPanel()
    {
        foreach (var item in m_display)
        {
            item.SetActive(IsActive);
        }
        IsActive = IsActive ? false : true;
        Debug.Log(IsActive);
    }
}
