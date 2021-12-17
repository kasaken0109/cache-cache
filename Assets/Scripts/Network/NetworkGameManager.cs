using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class NetworkGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    bool m_debugMode;
    [SerializeField, Tooltip("ハンターの最大人数")]
    int m_hunterCapacity;
    [SerializeField, Tooltip("ウィッチの最大人数")]
    int m_witchCapacity;
    [SerializeField]
    GameObject m_checkButton;
    public int HunterCapacity { get => m_hunterCapacity; }
    public int WitchCapacity { get => m_witchCapacity; }
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        Connect("1.0");// 1.0 はバージョン番号（同じバージョンを指定したクライアント同士が接続できる）
    }
    /// <summary>
    /// Photonに接続する
    /// </summary>
    private void Connect(string gameVersion)
    {
        StartCoroutine(ConnectAsync(gameVersion));
    }

    IEnumerator ConnectAsync(string gameVersion)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        while (PhotonNetwork.IsConnected)
        {
            yield return new WaitForEndOfFrame();
        }
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;    // 同じバージョンを指定したもの同士が接続できる
            PhotonNetwork.ConnectUsingSettings();
        }
        yield return null;
    }
    /// <summary>
    /// ロビーに入る
    /// </summary>
    private void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    /// <summary>
    /// 既に存在する部屋に参加する
    /// </summary>
    private void JoinExistingRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    /// <summary>
    /// ランダムな名前のルームを作って参加する
    /// </summary>
    private void CreateRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;   // 誰でも参加できるようにする
            roomOptions.MaxPlayers = (byte)(HunterCapacity + WitchCapacity);
            //ルーム名に null を指定するとランダムなルーム名を付ける
            PhotonNetwork.CreateRoom(null, roomOptions);
        }
    }
    private void CheckPlayerCountAndStartGame()
    {
        /* **************************************************
         * ルームに参加している人数が最大に達したら部屋を閉じる（参加を締め切る）
         * 部屋を閉じないと、最大人数から減った時に次のユーザーが入ってきてしまう。
         * 現状のコードではユーザーが最大人数から減った際の追加入室を考慮していないため、追加入室させたい場合は実装を変更する必要がある。
         * **************************************************/
        if (m_debugMode || PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > PhotonNetwork.CurrentRoom.MaxPlayers - 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Lobby, null, raiseEventoptions, sendOptions);
        }
    }

    /* ***********************************************
     * 
     * これ以降は Photon の Callback メソッド
     * 
     * ***********************************************/

    /// <summary>Photon に接続した時</summary>
    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    /// <summary>Photon との接続が切れた時</summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }

    /// <summary>マスターサーバーに接続した時</summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        JoinLobby();
    }

    /// <summary>ロビーに参加した時</summary>
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        JoinExistingRoom();
    }

    /// <summary>ロビーから出た時</summary>
    public override void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    /// <summary>部屋を作成した時</summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    /// <summary>部屋の作成に失敗した時</summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }

    /// <summary>部屋に入室した時</summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        m_checkButton.SetActive(true);
    }

    /// <summary>指定した部屋への入室に失敗した時</summary>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed: " + message);
    }

    /// <summary>ランダムな部屋への入室に失敗した時</summary>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed: " + message);
        CreateRandomRoom();
    }

    /// <summary>部屋から退室した時</summary>
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    /// <summary>自分のいる部屋に他のプレイヤーが入室してきた時</summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom: " + newPlayer.NickName);
        CheckPlayerCountAndStartGame();
    }
    /// <summary>自分のいる部屋から他のプレイヤーが退室した時</summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //自分がハンターかウィッチかの判定と自分が死んだかの判定をかく
        //退室したプレイヤーのオーナーIDが一致しているオブジェクトを呼び出したい
        //マスタークライアントが抜けた場合どうするか
        if (otherPlayer.IsMasterClient)
        {
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Win, null, raiseEventoptions, sendOptions);
        }
        else
        {
            var chara = new List<CharaBase>(FindObjectsOfType<CharaBase>());
            PhotonView view;
            foreach (var item in chara)
            {
                view = item.GetComponent<PhotonView>();
                var witch = item.GetComponent<Witch>();
                if (!witch.IsDead)
                {
                    RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
                    raiseEventoptions.Receivers = ReceiverGroup.All;
                    SendOptions sendOptions = new SendOptions();
                    PhotonNetwork.RaiseEvent((byte)NetworkEvents.Die, null, raiseEventoptions, sendOptions);
                }
            }
        }
        Debug.Log("OnPlayerLeftRoom: " + otherPlayer.NickName);
    }
    /// <summary>マスタークライアントが変わった時</summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched to: " + newMasterClient.NickName);
    }

    /// <summary>ロビー情報に更新があった時</summary>
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("OnLobbyStatisticsUpdate");
    }

    /// <summary>ルームリストに更新があった時</summary>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
    }

    /// <summary>ルームプロパティが更新された時</summary>
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("OnRoomPropertiesUpdate");
    }

    /// <summary>プレイヤープロパティが更新された時</summary>
    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("OnPlayerPropertiesUpdate");
    }
}
