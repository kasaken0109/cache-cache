using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public class TimeManager : MonoBehaviour
{
    //制限時間を表示するテキスト
    [SerializeField]
    Text m_timeText;
    //分
    public int m_minute;
    //秒
    int m_seconds;
    int[] m_alretTiem = new int[] { 5, 3, 1 };
    int m_alretCount = 0;

    public float m_secondTime;

    public bool m_isStarted;

    public void Stop(bool set) => m_isStarted = set;

    void Update()
    {
        if (m_isStarted)
        {
            m_secondTime -= Time.deltaTime;
            m_seconds = (int)m_secondTime;
            m_timeText.text = m_minute.ToString() + ":" + m_seconds.ToString();
            if (m_minute > 0 && m_seconds == 0)
            {
                m_minute--;
                m_secondTime = 60;
            }
            else if(m_minute <= 0 && m_seconds == 0)
            {
                Stop(false);
                RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
                raiseEventoptions.Receivers = ReceiverGroup.All;
                SendOptions sendOptions = new SendOptions();
                PhotonNetwork.RaiseEvent((byte)NetworkEvents.Win, null, raiseEventoptions, sendOptions);
            }

            if (m_alretTiem[m_alretCount] == m_minute)
            {
                m_alretCount++;
                RingAlert();
            }
        }
    }

    public void RingAlert()
    {
        var witches = new List<CharaBase>(FindObjectsOfType<CharaBase>()).
            Where(c => 
            { 
                if (c.gameObject.CompareTag("Witch")) return c;
                else return false;
            });
        List<CharaBase> set = new List<CharaBase>(witches);
        set.ForEach(w => 
        {
            PhotonView view = w.gameObject.GetComponent<PhotonView>();
            if (view.IsMine)
            {
                view.RPC("SpawnAlarm", RpcTarget.All);
                return;
            }
        });
    }
}
