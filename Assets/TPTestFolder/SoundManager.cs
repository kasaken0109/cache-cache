using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance = null;
    public static SoundManager Instance => _instance;

    [System.Serializable]
    class AudioData
    {
        public string Name;
        public int ID;
        public AudioClip Clip;
        public float Vol;
        public bool IsLoop;
    }

    [SerializeField] int m_poolCount;
    [SerializeField] List<AudioData> m_datas = new List<AudioData>();

    SoundPool<Sound> m_soundPool;

    private void Awake()
    {
        m_soundPool = new SoundPool<Sound>();
        m_soundPool.Create(m_poolCount);

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            return;
        }
        
        Destroy(gameObject);
    }

    /// <summary> 自身だけがSoundDataを使う </summary>
    /// <param name="name">Soundの名前</param>
    public static void SetSound(string name)
    {
        Debug.Log("SetSoundToSelf呼ばれた");
        foreach (AudioData data in Instance.m_datas)
        {
            Debug.Log("Data検索");
            if (name == data.Name)
            {
                Sound sound = Instance.m_soundPool.Use();
                object[] datas = { data.Clip, data.Vol, data.IsLoop };
                sound.SetUp(datas);
                sound.Play();
                Debug.Log("IsSetSound");
                return;
            }
        }

        Debug.LogError("一致するデータがありません");
    }

    /// <summary> 自身だけがSoundDataを使う </summary>
    /// <param name="id">SoundのID</param>
    public static void SetSound(int id)
    {
        foreach (AudioData data in Instance.m_datas)
        {
            if (id == data.ID)
            {
                Sound sound = Instance.m_soundPool.Use();
                object[] datas = { data.Clip, data.Vol, data.IsLoop };
                sound.SetUp(datas);
                sound.Play();
                return;
            }
        }

        Debug.LogError("一致するデータがありません");
    }
}

class SoundPool<T> where T : Sound
{
    int _poolIndex = 0;
    GameObject _parent = null;

    List<Sound> m_pool = new List<Sound>();

    public void Create(int poolCount)
    {
        if (_parent == null) _parent = new GameObject("Sounds");
        else Debug.Log("AddPool");
        
        for (int i = 0; i < poolCount; i++)
        {
            Sound se = new GameObject($"SE:{_poolIndex}").AddComponent<Sound>();
            se.gameObject.AddComponent<AudioSource>();
            m_pool.Add(se);
            se.gameObject.SetActive(false);
            se.transform.SetParent(_parent.transform);

            _poolIndex++;
        }
    }

    public Sound Use()
    {
        foreach (Sound sound in m_pool)
        {
            if (!sound.gameObject.activeSelf)
            {
                sound.gameObject.SetActive(true);
                return sound;
            }
        }

        // すべて使われている状態なら、Poolに追加
        Create(3);
        return Use();
    }
}
