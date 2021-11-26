using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionIndicator : MonoBehaviour
{
    [SerializeField] CharaBase m_setChara;
    // Start is called before the first frame update
    public float MyPosX => m_setChara.transform.position.x;

    private int m_myLayerNum = default;
    public int MyLayerNum { get => m_myLayerNum;set { m_myLayerNum = value; } }
}
