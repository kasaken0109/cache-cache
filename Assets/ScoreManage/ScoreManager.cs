using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Photon 用の名前空間を参照する
using Photon.Pun;

public class ScoreManager : MonoBehaviour
{
    // どこからでも呼び出せるように
    private static ScoreManager m_instance = null;
    public static ScoreManager Instance => m_instance;
    private void Awake() => m_instance = this;

    public class PlayerScoreData
    {
        public int GetID { get; private set; }
        public void SetID(int id) => GetID = id;

        public float GetScore { get; private set; }
        public void AddScore(float score) => GetScore += score;
        public void SetScore(float score) => GetScore = score;
    }

    [SerializeField] List<ScoreEventData> m_eventDatas = new List<ScoreEventData>();

    public List<PlayerScoreData> PlayersScore { get; private set; }
    PhotonView m_view;

    void Start()
    {
        m_view = GetComponent<PhotonView>();
        PlayersScore = new List<PlayerScoreData>();

        for (int count = 0; count < PhotonNetwork.PlayerList.Length; count++)
        {
            PlayerScoreData pScore = new PlayerScoreData();
            int id = PhotonNetwork.PlayerList[count].ActorNumber;
            pScore.SetID(id);
            PlayersScore.Add(pScore);
        }
    }

    public static void RequestAddScore(EventType type)
    {
        foreach (ScoreEventData data in Instance.m_eventDatas)
        {
            if (data.Type == type)
            {
                Instance.AddScore(data);
                return;
            }
        }
    }

    void AddScore(ScoreEventData data)
    {
        int id = PhotonNetwork.LocalPlayer.ActorNumber;
        foreach (PlayerScoreData pScore in PlayersScore)
        {
            if (pScore.GetID == id)
            {
                pScore.AddScore(data.Score);
                object[] datas = { pScore.GetID, pScore.GetScore };
                m_view.RPC(nameof(SendScore), RpcTarget.Others, datas);
                return;
            }
        }
    }

    [PunRPC]
    void SendScore(object[] datas)
    {
        int getID = (int)datas[0];
        float currentScore = (float)datas[1];
        
        foreach (PlayerScoreData pScore in PlayersScore)
        {
            if (pScore.GetID == getID)
            {
                pScore.SetScore(currentScore);
                return;
            }
        }
    }
}

[System.Serializable]
public class ScoreEventData
{
    public EventType Type;
    public float Score;
}

public enum EventType
{
    A,
    B,

    None,
}

