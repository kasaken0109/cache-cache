using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance{ get; private set; }

    [SerializeField]
    [Tooltip("生成するアイテムの一覧")]
    private GameObject[] m_items = default;

    [SerializeField]
    [Tooltip("重複を許すアイテムの個数")]
    private int m_count = 2;

    [SerializeField]
    [Tooltip("生成するアイテム位置")]
    private Transform[] m_spawnPos = default;

    private bool[] m_isExist;
    private int[] m_existCount;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_isExist = new bool[m_spawnPos.Length];
        m_existCount = new int[m_spawnPos.Length];

        for (int i = 0; i < m_spawnPos.Length; i++)
        {
            m_isExist[i] = false;
            m_existCount[i] = 2;
        }
    }

    public void SpawnItem(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var itemNum = Random.Range(0, m_items.Length);
            while (m_existCount[itemNum] == 0)
            {
                itemNum = Random.Range(0, m_items.Length);
            }
            var itemObj = m_items[itemNum];

            var posNum = Random.Range(0, m_spawnPos.Length);
            while (m_isExist[posNum] == true)
            {
                posNum = Random.Range(0, m_spawnPos.Length);
            }
            var pos = m_spawnPos[posNum];
            m_isExist[posNum] = true;
            m_existCount[itemNum]--;
            var g = PhotonNetwork.Instantiate(itemObj.name, pos.position, Quaternion.identity);
            g.GetComponent<ItemTypeGetter>().ID = itemNum;
        }
    }

    public void ResetItem(int category)
    {
        m_isExist[category] = false;
        if(m_existCount[category] < m_count)m_existCount[category]++;
    }
}
