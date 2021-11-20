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
    private static ShowTextCtrl _instance = new ShowTextCtrl();
    public static ShowTextCtrl Instance => _instance;

    public static void TestShow() => Instance.SetTextData();
    string _log;
    string _isName;
    List<string> _othersName = new List<string>();

    bool _isWitch = false;

    // Memo. 魔女視点
    public static void Show(NetworkEvents get) => Instance.SetTextData(get);
    
    void SetTextData(NetworkEvents get = NetworkEvents.Win)
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
                    if (obj.CompareTag("Witch")) _isWitch = true;
                }
                else if (!view.IsMine && item.CompareTag("Witch"))
                {
                    if (item.Name.Length <= 0)
                        item.Name = "Gest";
                    _othersName.Add(item.Name);
                }
            }
        }

        if (obj.CompareTag("Witch") && get == NetworkEvents.Win ||
            obj.CompareTag("Hunter") && get == NetworkEvents.Lose) _log = "Win";

        else
        {
            _log = "Lose";
        }
       
        _isName = PhotonNetwork.LocalPlayer.NickName;
        if (_isName.Length <= 0) _isName = "My";

        //SceneManager.LoadScene("TestResult");
    }

    public static string GetLogData() => Instance._log;
    public static string GetMyName() => Instance._isName;
    public static List<string> GetOhersName()
    {
        if (Instance._othersName.Count <= 0 || !Instance._isWitch) return null;
        else return Instance._othersName;
    }
}
