using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sounds;

public class SoundPlay : MonoBehaviour
{
    public int BGM;
    public int Id;

    private void Start()
    {
        SoundMaster.Request(null, BGM, 0);
    }

    public void Play()
    {
        SoundMaster.Request(null, Id, 0);
    }

    public void Play(int id)
    {
        SoundMaster.Request(null, id, 0);
    }
}
