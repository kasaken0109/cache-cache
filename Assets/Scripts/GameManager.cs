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
    public int m_charaCount;
    public bool m_readyCheck;
    PhotonView m_view;
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
    JudgementController judge;
    NetworkGameManager netManager;
    int randomNumber;
    bool IsFirst = true;
    bool FirstDeath = true;
    private void Start()
    {
        SceneManager.sceneLoaded += Spawn;
    }
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)NetworkEvents.Lobby:
                //judge.GameStartJudge(ref m_charaCount);
                int hunterNumber = PhotonNetwork.CurrentRoom.MaxPlayers + 1;
                randomNumber = Random.Range(1, hunterNumber);
                break;
            case (byte)NetworkEvents.GameStart:
                var scene = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
                scene.LoadScene("PlayerSyncTestKasai");
                break;
            case (byte)NetworkEvents.Win:
                Debug.Log("魔女の勝利");
                if (IsFirst) StartCoroutine(ShowResult(NetworkEvents.Win));
                break;
            case (byte)NetworkEvents.Lose:
                Debug.Log("魔女の負け");
                if (IsFirst) StartCoroutine(ShowResult(NetworkEvents.Lose));
                break;
            case (byte)NetworkEvents.Die:
                if (FirstDeath) StartCoroutine(DieCount());
                break;
        }
    }
    IEnumerator DieCount()
    {
        FirstDeath = false;
        yield return new WaitForSeconds(0.2f);
        judge = GameObject.Find("JudgementController").GetComponent<JudgementController>();
        netManager = GameObject.Find("GameManager").GetComponent<NetworkGameManager>();
        m_witchDieCount++;
        judge.LoseJudge(m_witchDieCount, netManager.WitchCapacity);
        Debug.Log("魔女が死んだ");
        FirstDeath = true;
    }

    IEnumerator ShowResult(NetworkEvents events)
    {
        IsFirst = false;
        ShowTextCtrl.Show(events);
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => ShowTextCtrl.GetLogData() != null);
        SceneManager.LoadScene("TestResult");
    }
    public void OnReady()
    {
        if (!judge && !m_view)
        {
            judge = GameObject.Find("JudgementController").GetComponent<JudgementController>();
            m_view = judge.GetComponent<PhotonView>();
        }
        if (!m_readyCheck)
        {
            m_view.RPC("Check", RpcTarget.All);
            m_readyCheck = true;
        }
        else
        {
            //var methodName = ((System.Action<int>)judge.UncChecked).Method.Name;
            //Debug.Log(methodName);
            m_view.RPC("UncChecked", RpcTarget.All);
            m_readyCheck = false;
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
