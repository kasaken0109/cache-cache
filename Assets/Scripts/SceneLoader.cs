using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //private IEnumerator LoadScene(string sceneName)
    //{
    //    var scene = SceneManager.LoadSceneAsync(sceneName);
    //    scene.allowSceneActivation = false;
    //    yield return new WaitForSeconds(2);
    //    scene.allowSceneActivation = true;
    //}
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
