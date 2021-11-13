using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharactorSpawn : MonoBehaviour
{
    [SerializeField, Tooltip("キャラクターの生成場所")]
    Transform[] m_charaPositions;
    //ハンターは魔法使い狩り
    [SerializeField]
    string m_hunterPrefabName = "PrefabName";
    [SerializeField]
    string m_witchPrefabName = "PrefabName";
    public Transform[] CharaPositions { get => m_charaPositions; }
    public GameObject Player { get; private set; }

    /// <summary>ハンターを生成するメソッド</summary>
    public void HunterSpawn(int number)
    {
        Transform spawnPoint = CharaPositions[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        // プレイヤーを生成し、他のクライアントと同期する
        Player = PhotonNetwork.Instantiate(m_hunterPrefabName, spawnPoint.position, spawnPoint.rotation);
        Player.GetComponent<Hunter>().SetUp();
        Player.GetComponent<PhotonView>().TransferOwnership(number);
        Debug.Log(number);
    }
    /// <summary>ウィッチを生成するメソッド</summary>
    public void WitchSpawn(int number)
    {
        Transform spawnPoint = CharaPositions[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        // プレイヤーを生成し、他のクライアントと同期する
        Player = PhotonNetwork.Instantiate(m_witchPrefabName, spawnPoint.position, spawnPoint.rotation);
        Player.GetComponent<Witch>().SetUp();
        Player.GetComponent<PhotonView>().TransferOwnership(number);
        Debug.Log(number);
    }
}
