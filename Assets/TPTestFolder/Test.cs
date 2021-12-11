using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class Test : MonoBehaviour
{
    public void Load()
    {
        SceneManager.LoadScene("New SceneSasaki");
    }

    public void SE()
    {
        Debug.Log("押した");
        SoundManager.SetSound("TestBGM");
    }
}
