using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimalManager : MonoBehaviour
{
    [SerializeField, Header("出現数")]
    int m_spawnNum = default;
    [SerializeField, Header("出現させる動物")]
    string m_animalName = default;

    Animal[] animals = default;
    Vector2Int m_scale = default;
    Vector2Int m_bottomLeft = default;
    Vector2Int m_topRight = default;
    List<Vector2> m_animalPos = new List<Vector2>();

    public void StartSpawn()
    {
        m_scale = new Vector2Int((int)this.transform.localScale.x, (int)this.transform.localScale.y);
        Vector2 pos = this.transform.position;
        m_bottomLeft = new Vector2Int((int)pos.x - m_scale.x / 2, (int)pos.y - m_scale.y / 2);
        m_topRight = new Vector2Int((int)pos.x + m_scale.x / 2, (int)pos.y + m_scale.y / 2);

        if (m_spawnNum >= m_scale.x * m_scale.y)
        {
            Debug.Log("設定された数が多すぎます");
            m_spawnNum = ((int)m_scale.x - 2) * ((int)m_scale.y - 2) - 1;
            Debug.Log(m_spawnNum + "を設定しました");
        }

        animals = new Animal[m_spawnNum];
        Spawn();
    }


    private void FixedUpdate()
    {
        bool a = false;
        foreach (var item in animals)
        {
            if (item == null)
            {
                a = true;
            }
        }
        if (a)
        {
            return;
        }

        foreach (var animal in animals)
        {
            animal.MovingUpdate();
        }
    }


    /// <summary>
    /// スポーンさせる
    /// </summary>
    private void Spawn()
    {
        for (int i = 0; i < m_spawnNum;)
        {
            bool dup = false;
            Vector2 pos = new Vector2(Random.Range(m_bottomLeft.x + 1, m_topRight.x), Random.Range(m_bottomLeft.y + 1, m_topRight.y));
            // 同じ場所にスポーンしないようにする
            for (int k = 0; k < m_animalPos.Count; k++)
            {
                if (m_animalPos[k] == pos)
                {
                    dup = true;
                    break;
                }
            }
            if (dup == true)
            {
                Debug.Log("not");
                continue;
            }
            animals[i] = PhotonNetwork.Instantiate(m_animalName, pos, Quaternion.identity).GetComponent<Animal>();
            animals[i].CreateStart();
            m_animalPos.Add(pos);
            i++;
        }
    } 
}
