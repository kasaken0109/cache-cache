using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class JudgementController : MonoBehaviour
{
    bool m_readyFlag;
    public void LoseJudge(int witchDieCount, int witchCapacity)
    {
        if (witchDieCount >= witchCapacity)
        {
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Lose, null, raiseEventoptions, sendOptions);
        }
    }
    public void GameStartJudge(ref int charaCount)
    {
        if (!m_readyFlag)
        {
            charaCount--;
        }
        else
        {
            charaCount++;
        }
        if (charaCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.GameStart, null, raiseEventoptions, sendOptions);
        }
        
    }
    public void Ready()
    {
        m_readyFlag = m_readyFlag? false : true;
        RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
        raiseEventoptions.Receivers = ReceiverGroup.All;
        SendOptions sendOptions = new SendOptions();
        PhotonNetwork.RaiseEvent((byte)NetworkEvents.Lobby, null, raiseEventoptions, sendOptions);
    }
}
