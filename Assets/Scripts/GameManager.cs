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
        SceneManager.sceneLoaded += Spawn;
    }
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)NetworkEvents.Lobby:
                var judge = GameObject.Find("JudgementController").GetComponent<JudgementController>();
                judge.GameStartJudge(ref m_charaCount);
                int hunterNumber = PhotonNetwork.CurrentRoom.MaxPlayers + 1;
                randomNumber = Random.Range(1, hunterNumber);
                break;
            case (byte)NetworkEvents.GameStart:
                var scene = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
                scene.LoadScene("PlayerSyncTestKasai");
                m_inGame = true;
                break;
            case (byte)NetworkEvents.Win:
                Debug.Log("魔女の勝利");
                ShowTextCtrl.Show(NetworkEvents.Win);
                break;
            case (byte)NetworkEvents.Lose:
                Debug.Log("魔女の負け");
                ShowTextCtrl.Show(NetworkEvents.Lose);
                break;
            case (byte)NetworkEvents.Die:
                Debug.Log("魔女が死んだ");
                judge = GameObject.Find("JudgementController").GetComponent<JudgementController>();
                var netManager = GameObject.Find("GameManager").GetComponent<NetworkGameManager>();
                m_witchDieCount++;
                judge.LoseJudge(m_witchDieCount, netManager.WitchCapacity);
                break;
        }
    }
    private void Spawn(Scene scene, LoadSceneMode mode)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var animalManager = GameObject.Find("AnimalManager").GetComponent<AnimalManager>();
            animalManager.StartSpawn();
            ItemManager.Instance.SpawnItem(4);
            var charactorSpawn = GameObject.Find("CharactorSpawn").GetComponent<CharactorSpawn>();
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                if (randomNumber != i + 1)
                {
                    Debug.Log("Witch");
                    charactorSpawn.WitchSpawn(i + 1);
                }
                else
                {
                    Debug.Log("Hunter");
                    charactorSpawn.HunterSpawn(i + 1);
                }
            }
        }
    }
}
