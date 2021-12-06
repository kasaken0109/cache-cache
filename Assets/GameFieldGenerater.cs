using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldGenerater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            var colObj = new GameObject($"collider{i}");
            colObj.transform.SetParent(this.gameObject.transform);
            colObj.transform.localScale = new Vector3(i % 2 == 0 ? 0.12f : 1,10f, i % 2 == 1 ? 0.12f : 1);
            colObj.transform.localPosition = new Vector3(i % 2 == 0 ? (i / 2 == 0 ?-0.56f : 0.56f) : 0, 0, i % 2 == 1 ? (i / 2 == 0 ? -0.56f : 0.56f) : 0);
            var col = colObj.AddComponent<BoxCollider>();
            //colObj.hideFlags = HideFlags.HideInHierarchy;
        }
    }
}
