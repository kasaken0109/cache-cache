using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharactorSpawn : MonoBehaviour
{
    enum CharaPosition
    {
        Hunter,
        Witch
    }
    [SerializeField, Tooltip("キャラクターの生成場所")]
    Transform[] m_charaPositions;
    [SerializeField]
    string m_hunterPrefabName = "PrefabName";
    [SerializeField]
    string m_witchPrefabName = "PrefabName";
    public Transform[] CharaPositions { get => m_charaPositions; }
    [PunRPC]
    public void CharaSpawn()
    {
        Debug.Log("キャラの生成");
        if (PhotonNetwork.IsMasterClient)
        {
            Transform spawnPoint = CharaPositions[(int)CharaPosition.Hunter];
            PhotonNetwork.Instantiate(m_hunterPrefabName, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Transform spawnPoint = CharaPositions[(int)CharaPosition.Witch];
            PhotonNetwork.Instantiate(m_witchPrefabName, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
