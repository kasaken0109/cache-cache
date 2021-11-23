using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeleportCreater : MonoBehaviour
{
    private enum ColliderType
    {
        Box,
        Circle,

        None,
    }

    [SerializeField] bool m_debug = false;
    [SerializeField] ColliderType m_type = ColliderType.None;
    [Tooltip("Circle基準の半径")]
    [SerializeField] float m_radius = 1;
    [Header("Pointerのデータ群. size内は個別のData")]
    [SerializeField] List<Setter> _setters = new List<Setter>();

    int m_groupID = 0;
    int m_individualID = 0;
    bool m_setUp = false;

    List<Dictionary<int, Transform>> m_tGroupID = new List<Dictionary<int, Transform>>();
    
    [System.Serializable]
    class Setter
    {
        public Vector2 Pointer1;
        public Vector2 Pointer2;
        public Vector2[] OthersPointer;
    }

    void Start()
    {
        if (m_debug) Create();
    }

    /// <summary>
    /// 任意での呼び出し
    /// </summary>
    public void Create()
    {
        if (m_setUp) return;
        if (m_type == ColliderType.None)
        {
            Debug.LogError("ColliderTypeの指定をして下さい");
            return;
        }
        m_setUp = true;
        _setters.ForEach(s => SetPointer(s));
    }

    /// <summary>
    /// 同じグループ内のテレポート先へ飛ばす
    /// </summary>
    /// <param name="key">GroupID</param>
    /// <param name="individualID">MyID</param>
    /// <param name="target">移動するObject</param>
    public void TPRequest(int key, int individualID, GameObject target)
    {
        List<Transform> targets = new List<Transform>(m_tGroupID[key].Values);
        bool set = false;
       
        while (!set)
        {
            int setId = Random.Range(0, targets.Count);
            if (setId != individualID)
            {
                set = true;
                target.transform.position = targets[setId].position;
            }
        }
    }

    void SetPointer(Setter s)
    {
        Dictionary<int, Transform> tPDic = new Dictionary<int, Transform>();

        CreateTrigger(s.Pointer1, tPDic);
        CreateTrigger(s.Pointer2, tPDic);
        if (s.OthersPointer.Length > 0) 
            new List<Vector2>(s.OthersPointer).ForEach(o => CreateTrigger(o, tPDic));

        m_tGroupID.Add(tPDic);
        m_groupID++;
    }

    void CreateTrigger(Vector2 setPos, Dictionary<int, Transform> key)
    {
        GameObject p = new GameObject($"TPPoint No.{m_groupID} : MyID.{m_individualID}");
        p.transform.position = setPos;
        Teleporter t = p.AddComponent<Teleporter>();
        t.GroupID = m_groupID;
        t.MyID = m_individualID;
        t.Creater = this;
        key.Add(m_individualID, p.transform);

        switch (m_type)
        {
            case ColliderType.Box:
                BoxCollider2D bCol = p.AddComponent<BoxCollider2D>();
                bCol.isTrigger = true;
                bCol.size = Vector2.one * (m_radius * 2);
                break;
            case ColliderType.Circle:
                CircleCollider2D cCol = p.AddComponent<CircleCollider2D>();
                cCol.isTrigger = true;
                cCol.radius = m_radius;
                break;
        }

        m_individualID++;
    }
}
