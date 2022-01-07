using UnityEngine;

namespace Sounds
{
    public class SoundEffect : MonoBehaviour, IPool
    {
        AudioSource _source;
        SEData _data;
        Transform _parent;
        public bool IsUse { get; private set; }

        void Update()
        {
            if (!IsUse) return;
            if (!_source.isPlaying) Delete();

            if (_source != null)
            {
                _source.volume = _data.Volume / SoundMaster.Instance.MasterVolumeRate;
                switch (_data.Type)
                {
                    case SoundType.BGM:
                        _source.volume /= SoundMaster.Instance.BGMVoluumeRate;
                        break;
                    case SoundType.SE:
                        _source.volume /= SoundMaster.Instance.SEVoluumeRate;
                        break;
                    case SoundType.None:
                        break;
                }
            }
        }

        public void Use(SEData data, Transform user)
        {
            _data = data;
            transform.SetParent(user);
            _source = GetComponent<AudioSource>();
            _source.clip = data.Clip;

            _source.volume = data.Volume / SoundMaster.Instance.MasterVolumeRate;
            switch (data.Type)
            {
                case SoundType.BGM:
                    _source.volume /= SoundMaster.Instance.BGMVoluumeRate;
                    break;
                case SoundType.SE:
                    _source.volume /= SoundMaster.Instance.SEVoluumeRate;
                    break;
                case SoundType.None:
                    break;
            }

            _source.spatialBlend = data.SpatialBrend;
            _source.loop = data.Loop;

            _source.Play();
            IsUse = true;
        }

        public void SetUp(Transform parent)
        {
            _source = gameObject.AddComponent<AudioSource>();
            _parent = parent;
            IsUse = false;
        }

        public void Delete()
        {
            IsUse = false;
            _source.clip = null;
            _source = null;
            _data = null;
            transform.SetParent(_parent);
        }
    }
}
