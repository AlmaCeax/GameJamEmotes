using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Robot>())
        {
            GameManager.Instance.OnReachFinishLine(PhotonNetwork.LocalPlayer);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Robot>())
        {
            GameManager.Instance.OnLeaveFinishLine(PhotonNetwork.LocalPlayer);
        }
    }
}
