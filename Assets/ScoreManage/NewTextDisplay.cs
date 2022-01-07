using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewTextDisplay : MonoBehaviour
{
    [SerializeField] Text m_witchesResurt;
    [SerializeField] Text m_witchesName;
    [SerializeField] Text m_hunterResult;
    [SerializeField] Text m_hunterName;

    void Start()
    {
        Init();

        m_witchesResurt.text = ResultUIData.GetResultWitch;
        m_hunterResult.text = ResultUIData.GetResultHunter;
        m_hunterName.text = ResultUIData.GetHunterText;
        
        foreach (var text in ResultUIData.GetWitchText)
        {
            m_witchesName.text += text + "\n";
        }
    }

    void Init()
    {
        m_witchesResurt.text = "";
        m_witchesName.text = "";
        m_hunterName.text = "";
        m_hunterResult.text = "";
    }
}
