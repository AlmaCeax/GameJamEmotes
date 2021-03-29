using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureActivator : MonoBehaviour
{
    public PointsPuzzle puzzle;
    public float pressureDisplacement = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        puzzle.Active = true;
        transform.position = transform.position + (Vector3.down * pressureDisplacement);
    }

    private void OnTriggerExit(Collider other)
    {
        puzzle.Active = false;
        transform.position = transform.position - (Vector3.down * pressureDisplacement);
    }
}
