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

    /// <summary>ハンターを生成するメソッド</summary>
    /// <param name="hunterPosition">キャラクターを生成する座標</param>
    public void HunterSpawn()
    {
        Transform spawnPoint = CharaPositions[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        // プレイヤーを生成し、他のクライアントと同期する
        GameObject m_hunter = PhotonNetwork.Instantiate(m_hunterPrefabName, spawnPoint.position, spawnPoint.rotation);
    }

    /// <summary>ウィッチを生成するメソッド</summary>
    /// <param name="witchPosition">キャラクターを生成する座標</param>
    public void WitchSpawn()
    {
        Transform spawnPoint = CharaPositions[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        // プレイヤーを生成し、他のクライアントと同期する
        GameObject m_witch = PhotonNetwork.Instantiate(m_witchPrefabName, spawnPoint.position, spawnPoint.rotation);
    }
}
