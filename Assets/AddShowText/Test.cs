using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] Text _nameText = null;
    [SerializeField] Text _logText = null;
    
    void Start()
    {
        if (_logText != null && _logText != null) SetText();
    }

    public void OnClick() => ShowTextCtrl.TestShow();

    void SetText()
    {
        _logText.text = ShowTextCtrl.GetLogData();
        _nameText.text = ShowTextCtrl.GetMyName();

        if (ShowTextCtrl.GetOhersName() == null) return;
        ShowTextCtrl.GetOhersName().ForEach(n => _nameText.text += "\n" + n);
    }
}
