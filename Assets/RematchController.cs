using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RematchController : MonoBehaviour
{
    [SerializeField]
    private SceneLoader m_sceneLoader = default;
    [SerializeField]
    private GameObject[] m_display = default;
    // Start is called before the first frame update
    public void StartRematch()
    {
        StartCoroutine(m_sceneLoader.LoadScene(0));
    }

    bool IsActive = true;
    public void SetPanel()
    {
        foreach (var item in m_display)
        {
            item.SetActive(IsActive);
        }
        IsActive = IsActive ? false : true;
        Debug.Log(IsActive);
    }
}
