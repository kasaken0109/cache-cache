using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class SceneLoader : MonoBehaviour
{
    public IEnumerator LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        scene.allowSceneActivation = true;
    }
    public IEnumerator LoadScene(int sceneIndex)
    {
        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        scene.allowSceneActivation = true;
    }
    public void StartLoadScene(string methodName,int sceneIndex)
    {
        StartCoroutine(methodName,sceneIndex);
    }
}
