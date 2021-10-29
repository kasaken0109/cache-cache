using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterCamera : MonoBehaviour
{
    [SerializeField]
    Transform m_type = default;
    [SerializeField]
    Transform m_rawImage = default;


    public void Rotation(float h, float v)
    {
        if (h == 0 && v == 0)
        {
            return;
        }

        Vector3 d = new Vector3(h, v);

        float dist = Vector3.SignedAngle(Vector3.right, d, Vector3.forward);


        m_type.rotation = Quaternion.Euler(0, 0, dist - 90);
        m_rawImage.localRotation = Quaternion.Euler(0, 0, -(dist - 90));
    }
}
