using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEngine.SceneManagement;
// Photon 用の名前空間を参照する
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class ShowTextCtrl
{
    private static ShowTextCtrl m_instance = new ShowTextCtrl();
    public static ShowTextCtrl Instance => m_instance;

    string m_log;
    string m_isName;
    List<string> m_othersName = new List<string>();

    bool m_isWitch = false;

    // Memo. 魔女視点
    public static void Show(NetworkEvents get) => Instance.SetTextData(get);
    
    void SetTextData(NetworkEvents get)
    {
        var charas = new List<CharaBase>(GameObject.FindObjectsOfType<CharaBase>());
        GameObject obj = default;
        PhotonView view;

        foreach (var item in charas)
        {
            if (item.CompareTag("Hunter") || item.CompareTag("Witch"))
            {
                view = item.gameObject.GetComponent<PhotonView>();
                if (view.IsMine)
                {
                    obj = item.gameObject;
                    if (obj.CompareTag("Witch")) m_isWitch = true;
                }
                else if (!view.IsMine && item.CompareTag("Witch"))
                {
                    if (item.Name.Length <= 0)
                        item.Name = "Gest";
                    m_othersName.Add(item.Name);
                }
            }
        }

        if (obj.CompareTag("Witch") && get == NetworkEvents.Win ||
            obj.CompareTag("Hunter") && get == NetworkEvents.Lose) m_log = "Win";
        else
        {
            m_log = "Lose";
        }
       
        m_isName = PhotonNetwork.LocalPlayer.NickName;
        if (m_isName.Length <= 0) m_isName = "My";
    }

    public static string GetLogData() => Instance.m_log;
    public static string GetMyName() => Instance.m_isName;
    public static List<string> GetOhersName()
    {
        if (Instance.m_othersName.Count <= 0 || !Instance.m_isWitch) return null;
        else return Instance.m_othersName;
    }
}
