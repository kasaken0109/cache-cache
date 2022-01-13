using UnityEngine;

namespace Sounds
{
    public class SoundEffect : MonoBehaviour, IPool
    {
        Transform _parent;
        CriAtomSource _criS;
        CriAtomExPlayer _cri;
        public bool IsUse { get; private set; }

        void Start()
        {
            _criS = GetComponent<CriAtomSource>();
            _cri = _criS.player;
        }

        void Update()
        {
            if (!IsUse) return;

            if (_cri.GetStatus() == CriAtomExPlayer.Status.PlayEnd)
            {
                Delete();
            }
            //if (_source != null)
            //{
            //    _source.volume = _data.Volume * SoundMaster.Instance.MasterVolumeRate;
            //    switch (_data.Type)
            //    {
            //        case SoundType.BGM:
            //            _source.volume *= SoundMaster.Instance.BGMVoluumeRate;
            //            break;
            //        case SoundType.SE:
            //            _source.volume *= SoundMaster.Instance.SEVoluumeRate;
            //            break;
            //        case SoundType.None:
            //            break;
            //    }
            //}
        }

        public void Use(SEData data, Transform user)
        {
            _criS = GetComponent<CriAtomSource>();
            _cri = _criS.player;

            transform.SetParent(user);
            Debug.Log(_criS);
            _criS.volume = data.Volume * SoundMaster.Instance.MasterVolumeRate;
            
            switch (data.Type)
            {
                case SoundType.BGM:
                    _criS.volume *= SoundMaster.Instance.BGMVoluumeRate;
                    break;
                case SoundType.SE:
                    _criS.volume *= SoundMaster.Instance.SEVoluumeRate;
                    break;
                case SoundType.ME:
                    _criS.volume *= SoundMaster.Instance.SEVoluumeRate;
                    break;
            }
            
            _criS.loop = data.Loop;
            _criS.cueSheet = data.CueSheetName;
            _criS.cueName = data.CueName;
            _criS.Play();
            IsUse = true;
        }

        public void SetUp(Transform parent)
        {
            _parent = parent;
            IsUse = false;
        }

        public void Delete()
        {
            IsUse = false;
            
            _criS.volume = 0;
            _criS.cueName = null;
            _criS.cueSheet = null;
            
            transform.SetParent(_parent);
        }
    }
}
