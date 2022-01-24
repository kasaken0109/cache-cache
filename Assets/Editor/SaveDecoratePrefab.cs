using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SaveDecoratePrefab : MonoBehaviour
{
    [SerializeField]
    private InputField input = default;

    [SerializeField]
    [Tooltip("生成したいデコレーションオブジェクト")]
    private GameObject m_create = default;

    private string m_objName = "Default";
    private GameObject m_obj;

    public void CreateRandomObject()
    {
        var obj = new GameObject();
        var size = transform.localScale;
        for (float i = 0.2f; i <= size.z - 0.2f; i+= 2f)
        {
            for (float k = 0.2f; k <= size.x - 0.2f; k += 2f)
            {
                var randomX = Random.Range(-0.6f, 0.6f);
                var randomZ = Random.Range(-0.6f, 0.6f);
                var randomScale = Random.Range(0.9f, 1.1f);
                var c = Instantiate(m_create, new Vector3(k - size.x / 2 + randomX, 1, i - size.z / 2 + randomZ), transform.rotation,obj.transform);
                c.transform.localScale = new Vector3(m_create.transform.localScale.x * randomScale, m_create.transform.localScale.y * randomScale, m_create.transform.localScale.z);
                c.isStatic = true;
            }
        }
        m_obj = obj;
    }

    public void SavePrefab()
    {
        m_obj.name = m_objName;
        m_obj.isStatic = true;
        PrefabUtility.SaveAsPrefabAsset(m_obj, "Assets/Prefabs/FieldObjects/" + m_objName + ".prefab");
    }

    public void SetName(string value) => m_objName = value;
}
