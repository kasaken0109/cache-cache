using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class JudgementController : MonoBehaviour
{
    [SerializeField]
    CharactorSpawn m_charactorSpawn;
    public void LoseJudge(int witchDieCount)
    {
        if (witchDieCount >= m_charactorSpawn.WitchPositions.Length)
        {
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Lose, null, raiseEventoptions, sendOptions);
        }
    }
    void GameStartJudge(ref int charaCount)
    {
        var count = m_charactorSpawn.WitchPositions.Length + m_charactorSpawn.HunterPositions.Length;
        if (charaCount >= count)
        {
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.GameStart, null, raiseEventoptions, sendOptions);
        }
        else
        {
            charaCount++;
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Lobby, null, raiseEventoptions, sendOptions);
        }
    }
}
