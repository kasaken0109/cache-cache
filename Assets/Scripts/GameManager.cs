using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public enum NetworkEvents : byte
{
    //ゲームスタート前のロビーにいる状態
    Lobby,
    GameStart,
    //魔女側の勝ち
    Win,
    //魔女側の負け
    Lose,
    //魔女が死んだ状態
    Die,
}
public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{  
    CharactorSpawn m_charactorSpawn;
    AnimalManager m_animalManager;
    /// <summary>このクラスのインスタンスが既にあるかどうか</summary>
    static bool m_isExists = false;
    int m_witchDieCount;
    int m_witchCurrentCount;
    void Awake()
    {
        if (m_isExists)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_isExists = true;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Start()
    {
        m_animalManager = GameObject.Find("AnimalManager").GetComponent<AnimalManager>();
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)NetworkEvents.Lobby:

                break;
            case (byte)NetworkEvents.GameStart:
                m_animalManager.StartSpawn();
                break;
            case (byte)NetworkEvents.Win:
                Debug.Log("魔女の勝利");

                break;
            case (byte)NetworkEvents.Lose:
                Debug.Log("魔女の負け");

                break;
            case (byte)NetworkEvents.Die:
                Debug.Log("魔女が死んだ");
                var judge = GameObject.Find("JudgementController").GetComponent<JudgementController>();
                m_witchDieCount++;
                judge.LoseJudge(m_witchDieCount);
                break;
        }
    }
}
