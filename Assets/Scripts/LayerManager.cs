using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerManager : MonoBehaviour
{
    public static LayerManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("ウィッチの場所を表示するUI")]
    private Text m_witchPos = default;
    private int m_activeLayer = default;
    public int ActiveLayer => m_activeLayer;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void SetWitchPos(string name)
    {
        m_witchPos.text = name;
    }
}
