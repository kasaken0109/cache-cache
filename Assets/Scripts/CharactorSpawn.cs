using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharactorSpawn : MonoBehaviour
{
    //ハンターは魔法使い狩り
    [SerializeField] string m_hunterPrefabName = "PrefabName";
    [SerializeField] string m_witchPrefabName = "PrefabName";
    int m_currentCapacity;

    /// <summary>
    /// ハンターを生成するメソッド
    /// </summary>
    /// <param name="actorNumber">プレイヤーが入ってきた順の番号</param>
    /// <param name="currentCapacity">現在部屋に入っているハンターの人数</param>
    /// <param name="charactorPosition">キャラクターを生成する座標</param>
    public void HunterSpawn(int actorNumber, Transform[] hunterPosition)
    {
        if (hunterPosition.Length > m_currentCapacity)
        {
            
            actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Transform spawnPoint = hunterPosition[m_currentCapacity];
            // プレイヤーを生成し、他のクライアントと同期する
            GameObject hunter = PhotonNetwork.Instantiate(m_hunterPrefabName, spawnPoint.position, spawnPoint.rotation);
            m_currentCapacity++;
        }
    }
    /// <summary>
    /// ウィッチを生成するメソッド
    /// </summary>
    /// <param name="actorNumber">プレイヤーがボタンを押した順の番号</param>
    /// <param name="currentCapacity">現在部屋に入っている魔女の人数</param>
    /// <param name="charactorPosition">キャラクターを生成する座標</param>
    public void WitchSpawn(int actorNumber, Transform[] witchPosition)
    {
        if (witchPosition.Length > m_currentCapacity)
        {
            actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            //Debug.Log(actorNumber);
            Transform spawnPoint = witchPosition[m_currentCapacity];
            // プレイヤーを生成し、他のクライアントと同期する
            GameObject witch = PhotonNetwork.Instantiate(m_witchPrefabName, spawnPoint.position, spawnPoint.rotation);
            m_currentCapacity++;
        }
    }
}
