using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public enum NetworkEvents : byte
{
    GameStart,
    //魔女側の勝ち
    Win,
    //魔女側の負け
    Lose,
    //魔女が死んだ状態
    Die,
}
public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField]
    CharactorSpawn m_charactorSpawn;
    int m_witchDieCount;

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)NetworkEvents.GameStart:
                break;
            case (byte)NetworkEvents.Win:
                Debug.Log("魔女の勝利");
                break;
            case (byte)NetworkEvents.Lose:
                Debug.Log("魔女の負け");
                break;
            case (byte)NetworkEvents.Die:
                Debug.Log("魔女が死んだ");
                m_witchDieCount++;
                LoseJudge();
                break;
        }
    }
    void LoseJudge()
    {
        if (m_witchDieCount >= m_charactorSpawn.WitchPositions.Length)
        {
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Lose, null, raiseEventoptions, sendOptions);
        }
    }
}
