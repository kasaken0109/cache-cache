using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterCamera : MonoBehaviour
{
    [SerializeField]
    Transform m_rawImage;
    [SerializeField]
    Transform m_Image;

    void Start()
    {
        
    }
    private void Update()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Rotation(h, v);
    }


    void Rotation(float h, float v)
    {
        if (h == 0 && v == 0)
        {
            return;
        }

        Vector3 d = new Vector3(h, v);

        float dist = Vector3.SignedAngle(Vector3.right, d, Vector3.forward);


        m_Image.rotation = Quaternion.Euler(0, 0, dist - 90);
        m_rawImage.localRotation = Quaternion.Euler(0, 0, -(dist - 90));
    }
}
