using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class GateController : MonoBehaviour
{
    int eacapeNum = 0;
    // Start is called before the first frame update

    private void Update()
    {
        CheckGate();
    }

    bool IsFirst = true;
    private void CheckGate()
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers - GameObject.FindObjectOfType<GameManager>().WitchDieCount - 1 == eacapeNum && IsFirst)
        {
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Win, null, raiseEventoptions, sendOptions);
            IsFirst = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hunter")) return;
        if (other.CompareTag("Witch"))
        {
            eacapeNum++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Witch"))
        {
            eacapeNum--;
        }
    }
}
