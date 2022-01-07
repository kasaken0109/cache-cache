using System.Collections.Generic;
using UnityEngine;

namespace Sounds
{
    public enum SoundType
    {
        BGM,
        SE,

        None,
    }

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
        public SoundType Type;
        public AudioClip Clip;
        [Range(0, 1)] public float Volume;
        [Range(0, 1)] public float SpatialBrend;
        public bool Loop;
    }
}
