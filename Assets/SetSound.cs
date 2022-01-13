using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSound : MonoBehaviour
{
    enum BGMType
    {
        Main = 0,
        Game = 1,
    }

    [SerializeField] BGMType _type;
    void Start()
    {
        Debug.Log((int)_type);
        Sounds.SoundMaster.Request(null, (int)_type, 0);
    }

    public void OnClick(int id)
    {
        Sounds.SoundMaster.Request(null, id, 1);
    }
}
