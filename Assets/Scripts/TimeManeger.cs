using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManeger : MonoBehaviour
{
    //制限時間を表示するテキスト
    [SerializeField]
    Text m_timeText;
    //分
    public int m_minute;
    //秒
    int m_seconds;

    public float m_secondTime;

    public bool m_isStarted;

    public void Stop()
    {
        m_isStarted = false;
    }

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
                Stop();
            }
        }
    }
}
