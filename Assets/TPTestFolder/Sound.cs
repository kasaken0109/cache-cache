using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// こちら側はいじる必要なし、多分
public class Sound : MonoBehaviour
{
    AudioSource _source = null;

    public void SetUp(object[] datas)
    {
        if (_source == null) _source = GetComponent<AudioSource>();

        _source.clip = (AudioClip)datas[0];
        _source.volume = (float)datas[1] / 100;
        _source.loop = (bool)datas[2];
    }

    public void Play()
    {
        _source.Play();
        StartCoroutine(PlayFnish());
    }

    IEnumerator PlayFnish()
    {
        while(_source.isPlaying) 
            yield return null;

        Delete();
    }

    void Delete()
    {
        gameObject.SetActive(false);
        _source = null;
    }
}
