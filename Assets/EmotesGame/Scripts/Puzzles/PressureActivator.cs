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
        if (!pView.IsMine)
        {
            Robot robot = other.GetComponent<Robot>();
            if (robot != null && !robot.pView.IsMine)
                return;
        }

        pView.RPC("OnStep", RpcTarget.All, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!pView.IsMine)
        {
            Robot robot = other.GetComponent<Robot>();
            if (robot != null && !robot.pView.IsMine)
                return;
        }

        pView.RPC("OnStep", RpcTarget.All, false);
    }

    [PunRPC]
    public void OnStep(bool mode)
    {
        puzzle.Active = mode;
        Vector3 movement = mode ? Vector3.down * pressureDisplacement : -Vector3.down * pressureDisplacement;
        transform.position = transform.position + movement;
    }
}
