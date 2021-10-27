using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestCall : MonoBehaviour
{
    public void OnFadeIn()
    {
        FadeControl.FadeIn();
    }

    public void OnFadeOut()
    {
        FadeControl.FadeOut();
    }
}
