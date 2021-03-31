using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrientation : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform, new Vector3(0, 1, 0));
        var curr = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, curr.y + 180f, 0);
    }
}
