using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("勝者名の表示箇所")]
    Text m_winner = default;

    [SerializeField]
    [Tooltip("敗者名の表示箇所")]
    Text m_loser = default;

    public void ShowResult()
    {
        m_winner.text = ShowTextCtrl.GetLogData();
        m_loser.text = ShowTextCtrl.GetLogData();
    }
}
