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
    public int WitchDieCount { get; set; }
    bool m_readyCheck;
    PhotonView m_view;
    [SerializeField]
    GameObject m_checkImage;
    SceneLoader scene;
    JudgementController judge;
    NetworkGameManager netManager;
    int randomNumber;
    bool IsFirst = true;
    bool FirstDeath = true;
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
        SceneManager.sceneLoaded += Spawn;
    }
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)NetworkEvents.Lobby:
                
                break;
            case (byte)NetworkEvents.GameStart:
                scene = GetComponent<SceneLoader>();
                StartCoroutine(SpawnLoadScene(1));
                Debug.Log(scene);
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
        WitchDieCount++;
        judge.LoseJudge(WitchDieCount, netManager.WitchCapacity);
        Debug.Log("魔女が死んだ");
        FirstDeath = true;
    }

    IEnumerator ShowResult(NetworkEvents events)
    {
        IsFirst = false;
        ShowTextCtrl.Show(events);
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => ShowTextCtrl.GetLogData() != null);
        scene = GetComponent<SceneLoader>();
        StartCoroutine(scene.LoadScene(2));
    }
    public void OnReady()
    {
        if (!m_view)
        {
            m_view = GameObject.Find("JudgementController").GetComponent<PhotonView>();
        }
        if (!m_readyCheck)
        {
            m_view.RPC("Check", RpcTarget.All);
            m_readyCheck = true;
            m_checkImage.SetActive(true);
        }
        else
        {
            m_view.RPC("UncChecked", RpcTarget.All);
            m_readyCheck = false;
            m_checkImage.SetActive(false);
        }
    }
    private void Spawn(Scene scene, LoadSceneMode mode)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var animalManager = GameObject.Find("AnimalManager").GetComponent<AnimalManager>();
            animalManager.StartSpawn();
            ItemManager.Instance.SpawnItem(4);
        }
    }
    //ゲームシーンに遷移するときに使う
    IEnumerator SpawnLoadScene(int sceneIndex)
    {
        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;
        yield return new WaitForSeconds(2);
        scene.allowSceneActivation = true;
        yield return new WaitForSeconds(0.1f);
        //マスタークライアントを変えることでハンターをランダムにできる
        int hunterNumber = PhotonNetwork.CurrentRoom.MaxPlayers;
        randomNumber = Random.Range(0, hunterNumber);
        var player = PhotonNetwork.PlayerList[randomNumber];
        Debug.Log(player.ActorNumber);
        PhotonNetwork.SetMasterClient(player);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        while (!player.IsMasterClient)
        {
            yield return null;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            var chara = GameObject.Find("CharactorSpawn").GetComponent<CharactorSpawn>();
            var view = chara.GetComponent<PhotonView>();
            view.RPC(nameof(chara.CharaSpawn), RpcTarget.All);
        }
    }
}
