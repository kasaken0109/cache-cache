using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    string m_sceneName;
    [SerializeField]
    int m_sceneIndex;

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
    public IEnumerator LoadSceneName()
    {
        var scene = SceneManager.LoadSceneAsync(m_sceneName);
        scene.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        scene.allowSceneActivation = true;
    }
    public IEnumerator LoadSceneIndex()
    {
        var scene = SceneManager.LoadSceneAsync(m_sceneIndex);
        scene.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        scene.allowSceneActivation = true;
    }
}
