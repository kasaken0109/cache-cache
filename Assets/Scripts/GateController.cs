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

    private void CheckGate()
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers - 1 == eacapeNum)
        {
            RaiseEventOptions raiseEventoptions = new RaiseEventOptions();
            raiseEventoptions.Receivers = ReceiverGroup.All;
            SendOptions sendOptions = new SendOptions();
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.Win, null, raiseEventoptions, sendOptions);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
