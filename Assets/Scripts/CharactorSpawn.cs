using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharactorSpawn : MonoBehaviour
{
    //ハンターは魔法使い狩り
    [SerializeField] string m_hunterPrefabName = "PrefabName";
    [SerializeField] string m_witchPrefabName = "PrefabName";
    [SerializeField] int m_maxHunters;
    [SerializeField] int m_maxWitches;
    public void HunterSpawn(int actorNumber,Transform[] charactorPosition)
    {
        Transform spawnPoint = charactorPosition[actorNumber - 1];
        // プレイヤーを生成し、他のクライアントと同期する
        if (m_maxHunters >= 0)
        {
            GameObject hunter = PhotonNetwork.Instantiate(m_hunterPrefabName, spawnPoint.position, spawnPoint.rotation);
            m_maxHunters--;
        }
    }
    public void WitchSpawn(int actorNumber,Transform[] charactorPosition)
    {
        Transform spawnPoint = charactorPosition[actorNumber - 1];
        // プレイヤーを生成し、他のクライアントと同期する
        if (m_maxWitches >= 0)
        {
            GameObject witch = PhotonNetwork.Instantiate(m_witchPrefabName, spawnPoint.position, spawnPoint.rotation);
            m_maxWitches--;
        }
    }
}
