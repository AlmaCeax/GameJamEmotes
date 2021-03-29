using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureActivator : MonoBehaviour
{
    public PointsPuzzle puzzle;
    public float pressureDisplacement = 0.5f;
    public PhotonView pView;

    private void OnTriggerEnter(Collider other)
    {
        if (!pView.Owner.IsMasterClient)
            return;

        puzzle.Active = true;
        transform.position = transform.position + (Vector3.down * pressureDisplacement);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!pView.Owner.IsMasterClient)
            return;
        
        puzzle.Active = false;
        transform.position = transform.position - (Vector3.down * pressureDisplacement);
    }
}
