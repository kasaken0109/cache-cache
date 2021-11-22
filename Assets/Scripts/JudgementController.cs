﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class JudgementController : MonoBehaviour
{
    [SerializeField] Image m_checkImage;
    public int m_charaCount;
    public void LoseJudge(int witchDieCount, int witchCapacity)
    {
        if (witchDieCount >= witchCapacity)
        {
            Debug.Log(witchDieCount);
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Lose, null, raiseEventoptions, sendOptions);
        }
    }
    [PunRPC]
    public void Check()
    {
        m_charaCount++;
        m_checkImage.gameObject.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == m_charaCount)
            {
                RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
                raiseEventoptions.Receivers = ReceiverGroup.All;
                SendOptions sendOptions = new SendOptions();
                PhotonNetwork.RaiseEvent((byte)NetworkEvents.GameStart, null, raiseEventoptions, sendOptions);
            }
        }
    }
    [PunRPC]
    public void UncChecked()
    {
        m_charaCount--;
        m_checkImage.gameObject.SetActive(false);
    }
}
