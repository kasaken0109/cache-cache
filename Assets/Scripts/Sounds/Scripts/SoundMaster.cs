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

        [SerializeField] int _createCount;
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
            _pool.SetUp(_se, transform, _createCount);
        }

        /// <summary>
        /// SE��Request
        /// </summary>
        /// <param name="user">�g���l</param>
        /// <param name="id">SEData��ID</param>
        /// <param name="groupID">SEDataBase��ID</param>
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
        /// SE��Request
        /// </summary>
        /// <param name="user">�g���l</param>
        /// <param name="name">SEData��Name</param>
        /// <param name="groupID">SEDataBase��ID</param>
        public static void Request(Transform user, string name, int groupID = 0)
        {
            SEDataBase dataBase = Instance._dataBases[groupID];
            foreach (SEData se in dataBase.GetData)
            {
                if (se.CueName == name)
                {
                    SoundEffect sound = Instance._pool.Respons();
                    sound.Use(se, user);
                    return;
                }
            }
        }
    }
}