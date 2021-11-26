﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimalManager : MonoBehaviour
{
    enum AnimalList
    {
        くま, うし, きつね, にわとり, animal
    }

    [System.Serializable]
    struct SpownAnimal
    {
        [SerializeField]
        public AnimalList animal;
        [SerializeField]
        public int mapNum;
        [SerializeField]
        public int right;
        [SerializeField]
        public int left;
        [SerializeField]
        public int spownNum;
    }
    List<Animal> animals = new List<Animal>();

    [SerializeField, Header("出現するz座標")]
    int[] m_mapPos = new int[] { 0, -20, -40};

    [SerializeField, Header("出現させる動物")]
    SpownAnimal[] m_spownAnimals = new SpownAnimal[] { };

    public void StartSpawn()
    {
        Spawn();
        StartCoroutine(ManagerUpdate());
    }

    IEnumerator ManagerUpdate()
    {
        while (true)
        {
            foreach (var animal in animals)
            {
                animal.MovingUpdate();
            }
            yield return new WaitForFixedUpdate();
        }
    }


    /// <summary>
    /// スポーンさせる
    /// </summary>
    private void Spawn()
    {
        bool samePos = false;
        int pos = 0;
        for (int i = 0; i < m_spownAnimals.Length; samePos = false)
        {
            if (m_spownAnimals[i].spownNum > m_spownAnimals[i].right - m_spownAnimals[i].left) 
            {
                Debug.LogError("設定している数を生成できません");
                break;
            }

            pos = Random.Range(m_spownAnimals[i].left, m_spownAnimals[i].right);
            // 同じ位置にならないようにする
            for (int k = 0; k < animals.Count; k++)
            {
                if (animals[k].gameObject.transform.position.z == m_mapPos[m_spownAnimals[i].mapNum] &&
                    pos == animals[k].gameObject.transform.position.x) samePos = true;
            }
            if (samePos) continue;

            animals.Add(PhotonNetwork.Instantiate(m_spownAnimals[i].animal.ToString(), new Vector3(pos, 0, m_mapPos[m_spownAnimals[i].mapNum]), Quaternion.identity).GetComponent<Animal>());

            i++;
        }
    } 
}
