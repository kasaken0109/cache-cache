using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HunterCamera : MonoBehaviour
{
    [SerializeField]
    Transform m_fov = default;
    [SerializeField]
    Transform m_rawImage = default;
    [SerializeField]
    float m_changeTime = default;

    float m_dist = default;
    float m_fovZ = default;

    private void Start()
    {
        StartCoroutine(FoVChange(10));
    }

    public void Rotation(float h, float v)
    {
        if (h == 0 && v == 0)
        {
            return;
        }

        Vector3 d = new Vector3(h, v);

        m_dist = Vector3.SignedAngle(Vector3.right, d, Vector3.forward) - 90;

        CameraRotationUpdate();
    }

    void CameraRotationUpdate()
    {
        m_fov.localRotation = Quaternion.Euler(0, 0, m_dist + m_fovZ);
        m_rawImage.localRotation = Quaternion.Euler(0, 0, -m_dist - m_fovZ);
    }

    public IEnumerator FoVChange(int time)
    {
        float z = 36;

        float a = m_changeTime;
        while (true)
        {
            a -= Time.deltaTime;
            m_fovZ = z * (a / m_changeTime);
            CameraRotationUpdate();
            m_fov.gameObject.GetComponent<Image>().fillAmount = 0.5f - (0.2f * (a / m_changeTime));

            if (a <= 0) break;
            yield return null;
        }

        yield return new WaitForSeconds(time);

        a = 0;
        while (true)
        {
            a += Time.deltaTime;
            m_fovZ = z * (a / m_changeTime);
            CameraRotationUpdate();
            m_fov.gameObject.GetComponent<Image>().fillAmount = 0.5f - (0.2f * (a / m_changeTime));

            if (a >= m_changeTime) break;
            yield return null;
        }

        yield return null;
    }
}
