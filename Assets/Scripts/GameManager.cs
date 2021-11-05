using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    //[SerializeField] NetworkGameManager m_netManager;
    [SerializeField] JudgementController m_judgement;
    /// <summary>このクラスのインスタンスが既にあるかどうか</summary>
    static bool m_isExists;
    int m_witchDieCount;
    int m_charaCount;
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
    int randomNumber;
    bool m_inGame;
    private void Start()
    {
        //SceneManager.sceneLoaded += AnimalSpawn(Scene scene,LoadSceneMode mode);
    }
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)NetworkEvents.Lobby:
                m_judgement.GameStartJudge(ref m_charaCount);
                int m_hunterNumber = PhotonNetwork.CurrentRoom.MaxPlayers + 1;
                randomNumber = Random.Range(1, m_hunterNumber);
                break;
            case (byte)NetworkEvents.GameStart:
                var scene = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
                scene.LoadScene("MainScene");
                m_inGame = true;
                Spawn();
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
                var netManager = GameObject.Find("NetworkGameManager").GetComponent<NetworkGameManager>();
                m_witchDieCount++;
                judge.LoseJudge(m_witchDieCount, netManager.WitchCapacity);
                break;
        }
    }
    private void Spawn()
    {
        if (m_inGame && PhotonNetwork.IsMasterClient)
        {
            var animalManager = GameObject.Find("AnimalManager").GetComponent<AnimalManager>();
            animalManager.StartSpawn();
            //var charactorSpawn = GameObject.Find("CharactorSpawn").GetComponent<CharactorSpawn>();
        }
    }
}
