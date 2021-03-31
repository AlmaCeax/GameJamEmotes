using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureActivator : MonoBehaviour
{
    public PointsPuzzle puzzle;
    public float pressureDisplacement = 0.5f;
    public PhotonView pView;

    int pressed = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!pView.IsMine)
        {
            Robot robot = other.GetComponent<Robot>();
            if (robot != null && !robot.pView.IsMine)
                return;
        }

        pView.RPC("OnStep", RpcTarget.All);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!pView.IsMine)
        {
            Robot robot = other.GetComponent<Robot>();
            if (robot != null && !robot.pView.IsMine)
                return;
        }

        pView.RPC("OnUnStep", RpcTarget.All);
    }

    [PunRPC]
    public void OnStep()
    {
        if (pressed == 0)
        {
            puzzle.Active = true;
            Vector3 movement = Vector3.down * pressureDisplacement;
            transform.position = transform.position + movement;
        }

        pressed++;
    }

    public void OnUnStep()
    {
        pressed--;

        if(pressed == 0)
        {
            puzzle.Active = false;
            Vector3 movement = -Vector3.down * pressureDisplacement;
            transform.position = transform.position + movement;
        }
    }
}
