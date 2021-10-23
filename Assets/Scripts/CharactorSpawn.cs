using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharactorSpawn : MonoBehaviour
{
    //ハンターは魔法使い狩り
    [SerializeField]
    string m_hunterPrefabName = "PrefabName";
    [SerializeField]
    string m_witchPrefabName = "PrefabName";
    //ハンターとウィッチのポジションの数を足した数が部屋の最大人数
    [SerializeField, Tooltip("ハンターの生成場所")]
    Transform[] m_hunterPositions;
    [SerializeField, Tooltip("ウィッチの生成場所")]
    Transform[] m_witchPositions;

    public Transform[] HunterPositions { get => m_hunterPositions; }
    public Transform[] WitchPositions { get => m_witchPositions; }
    GameObject m_hunter;
    GameObject m_witch;
    int m_currentCapacity;
    bool m_spwanFlag = true;
    /// <summary>
    /// ハンターを生成するメソッド
    /// </summary>
    /// <param name="actorNumber">プレイヤーが入ってきた順の番号</param>
    /// <param name="hunterPosition">キャラクターを生成する座標</param>
    public void HunterSpawn(int actorNumber, Transform[] hunterPosition)
    {
        if (hunterPosition.Length > m_currentCapacity && m_spwanFlag)
        {
            actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Transform spawnPoint = hunterPosition[m_currentCapacity];
            // プレイヤーを生成し、他のクライアントと同期する
            m_hunter = PhotonNetwork.Instantiate(m_hunterPrefabName, spawnPoint.position, spawnPoint.rotation);
            m_currentCapacity++;
            m_spwanFlag = false;
        }
    }
    /// <summary>
    /// ウィッチを生成するメソッド
    /// </summary>
    /// <param name="actorNumber">プレイヤーがボタンを押した順の番号</param>
    /// <param name="witchPosition">キャラクターを生成する座標</param>
    public void WitchSpawn(int actorNumber, Transform[] witchPosition)
    {
        if (witchPosition.Length > m_currentCapacity && m_spwanFlag)
        {
            actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Transform spawnPoint = witchPosition[m_currentCapacity];
            // プレイヤーを生成し、他のクライアントと同期する
            m_witch = PhotonNetwork.Instantiate(m_witchPrefabName, spawnPoint.position, spawnPoint.rotation);
            m_currentCapacity++;
            m_spwanFlag = false;
        }
    }
}
