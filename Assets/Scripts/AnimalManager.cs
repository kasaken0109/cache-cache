using System.Collections;
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
        //[SerializeField]
        //public int mapNum;
        [SerializeField]
        public Vector3 CenterPos;
        [SerializeField]
        public Vector3 Range;
        //[SerializeField]
        //public int right;
        //[SerializeField]
        //public int left;
        [SerializeField]
        public int spawnNum;
    }
    List<Animal> animals = new List<Animal>();

    [SerializeField, Header("出現させる動物")]
    SpownAnimal[] m_spawnAnimals = new SpownAnimal[] { };


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
        float posX;
        float posZ;
        int count;// 無限ループをさせない
        for (int i = 0; i < m_spawnAnimals.Length; i++)
        {
            count = 0;
            // spownNumの数spownさせる
            for (int n = 0; n < m_spawnAnimals[i].spawnNum; samePos = false)
            {
                count++;
                if (count >= 10) 
                {
                    break; 
                }

                posX = Random.Range(m_spawnAnimals[i].CenterPos.x, m_spawnAnimals[i].Range.x);
                posZ = Random.Range(m_spawnAnimals[i].CenterPos.z, m_spawnAnimals[i].Range.z);

                Vector3 pos = new Vector3(posX, m_spawnAnimals[i].CenterPos.y, posZ);
                // 同じ位置にならないようにする
                for (int k = 0; k < animals.Count; k++)
                {
                    Vector3 a = animals[k].gameObject.transform.position;
                    BoxCollider c = animals[k].gameObject.GetComponent<BoxCollider>();
                    a += c.center;
                    if (a.x + c.size.x > pos.x && a.x - c.size.x < pos.x &&
                        a.z + c.size.z > pos.z && a.z - c.size.z < pos.z)
                    {
                        samePos = true;
                        break;
                    }
                }
                if (samePos) continue;

                animals.Add(PhotonNetwork.Instantiate(m_spawnAnimals[i].animal.ToString(), new Vector3(posX, m_spawnAnimals[i].CenterPos.y + 2, posZ), Quaternion.identity).GetComponent<Animal>());
                n++;
            }
        }
    } 
}
