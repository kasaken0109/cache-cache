﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Photon.Pun;

public class ResultUIData
{
    private static ResultUIData m_instance = new ResultUIData();
    public static ResultUIData Instance => m_instance;

    string m_resultWitch = null;
    public static string GetResultWitch => Instance.m_resultWitch;

    string m_resultHunter = null;
    public static string GetResultHunter => Instance.m_resultHunter;

    string m_hunterText;
    public static string GetHunterText => Instance.m_hunterText;

    List<string> m_witchesText = new List<string>();
    public static List<string> GetWitchText => Instance.m_witchesText;

    // 魔女目線
    public static void RequestSetDatas(NetworkEvents events) => Instance.SetDatas(events);

    void SetDatas(NetworkEvents events)
    {
        if (events == NetworkEvents.Win)
        {
            m_resultWitch = "Win";
            m_resultHunter = "Lose";
        }
        else if (events == NetworkEvents.Lose)
        {
            m_resultWitch = "Lose";
            m_resultHunter = "Win";
        }

        var charas = GameObject.FindObjectsOfType<CharaBase>()
            .Where(c => c.gameObject.CompareTag("Witch") || c.gameObject.CompareTag("Hunter"));

        foreach (var id in ScoreManager.Instance.PlayersScore)
        {
            foreach (CharaBase c in charas)
            {
                if (c.gameObject.GetComponent<PhotonView>().OwnerActorNr == id.GetID)
                {
                    string name;
                    if (c.GetComponent<PhotonView>().IsMine) name = "My";
                    else name = "Other";

                    if (c.gameObject.CompareTag("Witch"))
                    {
                        string set = $"Name {name}: Score {id.GetScore}";
                        m_witchesText.Add(set);
                    }
                    else
                    {
                        m_hunterText = $"Name {name}: Score {id.GetScore}";
                    }
                    continue;
                }
            }
        }
    }
}