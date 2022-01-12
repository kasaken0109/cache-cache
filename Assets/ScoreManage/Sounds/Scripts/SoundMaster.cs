using System.Collections.Generic;
using UnityEngine;

namespace Sounds
{
    public class SoundMaster : MonoBehaviour
    {
        private static SoundMaster _instance = null;
        public static SoundMaster Instance => _instance;

        [SerializeField, Range(0, 1)] float _volumRate = 1;
        [SerializeField, Range(0, 1)] float _bgmVolumRate = 1;
        [SerializeField, Range(0, 1)] float _seVolumRate = 1;

        [SerializeField] SoundEffect _se;
        [SerializeField] List<SEDataBase> _dataBases;

        ObjectPool<SoundEffect> _pool;
        public float MasterVolumeRate { get => _volumRate; set { _volumRate = value; } }
        public float BGMVoluumeRate { get => _bgmVolumRate; set { _bgmVolumRate = value; } }
        public float SEVoluumeRate { get => _seVolumRate; set { _seVolumRate = value; } }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _pool = new ObjectPool<SoundEffect>();
            _pool.SetUp(_se, transform);
        }

        /// <summary>
        /// SEのRequest
        /// </summary>
        /// <param name="user">使う人</param>
        /// <param name="id">SEDataのID</param>
        /// <param name="groupID">SEDataBaseのID</param>
        public static void Request(Transform user, int id, int groupID = 0)
        {
            SEDataBase dataBase = Instance._dataBases[groupID];
            foreach (SEData se in dataBase.GetData)
            {
                if (se.ID == id)
                {
                    SoundEffect sound = Instance._pool.Respons();
                    sound.Use(se, user);
                    return;
                }
            }
        }

        /// <summary>
        /// SEのRequest
        /// </summary>
        /// <param name="user">使う人</param>
        /// <param name="name">SEDataのName</param>
        /// <param name="groupID">SEDataBaseのID</param>
        public static void Request(Transform user, string name, int groupID = 0)
        {
            SEDataBase dataBase = Instance._dataBases[groupID];
            foreach (SEData se in dataBase.GetData)
            {
                if (se.Name == name)
                {
                    SoundEffect sound = Instance._pool.Respons();
                    sound.Use(se, user);
                    return;
                }
            }
        }
    }
}
