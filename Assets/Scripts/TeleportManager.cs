using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    private enum ColliderType
    {
        Box,
        Circle,

        None,
    }

    [SerializeField] float m_coolTime;
    [SerializeField] ColliderType m_type = ColliderType.None;
    [SerializeField] GameObject m_paticleObj = null;
    [SerializeField] Material m_defM = default;
    [SerializeField, Tooltip("Circle基準の半径")] float m_radius = 1;
    [SerializeField, Header("Pointerのデータ群. size内は個別のData")] List<Setter> _setters = new List<Setter>();

    int m_groupID = 0;
    int m_individualID;
    bool m_setUp = false;
    bool m_request = false;

    List<Dictionary<int, Transform>> m_tGroupID = new List<Dictionary<int, Transform>>();
    
    [System.Serializable]
    class Setter
    {
        public Vector3 Pointer1;
        public Vector3 Pointer2;
        public Vector3[] OthersPointer;
    }

    void Start() => Create();

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
        if (m_request) return;
        m_request = true;
        List<Transform> targets = new List<Transform>(m_tGroupID[key].Values);
        bool set = false;
       
        while (!set)
        {
            int setId = Random.Range(0, targets.Count);
            if (setId != individualID)
            {
                set = true;
                StartCoroutine(CoolTime());
                target.transform.position = targets[setId].position;
            }
        }
    }

    IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(m_coolTime);
        m_request = false;
    }

    void SetPointer(Setter s)
    {
        Dictionary<int, Transform> tPDic = new Dictionary<int, Transform>();
        m_individualID = 0;

        CreateTrigger(s.Pointer1, tPDic);
        CreateTrigger(s.Pointer2, tPDic);
        if (s.OthersPointer.Length > 0) 
            new List<Vector3>(s.OthersPointer).ForEach(o => CreateTrigger(o, tPDic));

        m_tGroupID.Add(tPDic);
        m_groupID++;
    }

    void CreateTrigger(Vector3 setPos, Dictionary<int, Transform> key)
    {
        GameObject p = new GameObject($"TPPoint No.{m_groupID} : MyID.{m_individualID}");
        
        p.transform.position = setPos;
        CreateParticle(setPos).transform.SetParent(p.transform);

        Teleporter t = p.AddComponent<Teleporter>();
        t.GroupID = m_groupID;
        t.MyID = m_individualID;
        t.Creater = this;
        key.Add(m_individualID, p.transform);

        switch (m_type)
        {
            case ColliderType.Box:
                BoxCollider bCol = p.AddComponent<BoxCollider>();
                bCol.isTrigger = true;
                bCol.size = Vector3.one * (m_radius * 2);
                break;
            case ColliderType.Circle:
                SphereCollider sCol = p.AddComponent<SphereCollider>();
                sCol.isTrigger = true;
                sCol.radius = m_radius;
                break;
        }

        m_individualID++;
    }

    GameObject CreateParticle(Vector3 setPos)
    {
        GameObject obj = default;

        if (m_paticleObj != null)
        {
            obj = Instantiate(m_paticleObj);
            obj.transform.position = setPos;
            return obj;
        }

        obj = new GameObject("Particle");
        obj.transform.position = setPos;
        ParticleSystem p = obj.AddComponent<ParticleSystem>();
        p.Stop();
        ParticleSystem.MainModule main = p.main;
        main.duration = 0.05f;
        main.startLifetime = 2f;
        main.startSpeed = 0.5f;

        ParticleSystem.ShapeModule s = p.shape;
        s.shapeType = ParticleSystemShapeType.Circle;
        p.GetComponent<ParticleSystemRenderer>().material = m_defM;
        p.Play();
        return obj;
    }
}
