using System.Collections.Generic;
using UnityEngine;

namespace Sounds
{
    [CreateAssetMenu(fileName = "SEDatas")]
    public class SEDataBase : ScriptableObject
    {
        [SerializeField] List<SEData> _datas;
        public List<SEData> GetData => _datas;
    }

    [System.Serializable]
    public class SEData
    {
        public string Name;
        public int ID;
        public AudioClip Clip;
        [Range(0, 10)] public float Volume;
        [Range(0, 1)] public float SpatialBrend;
        public bool Loop;
    }
}
