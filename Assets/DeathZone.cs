using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Robot robot = other.GetComponent<Robot>();
        if (robot != null && robot.pView.IsMine)
        {
            GameManager.Instance.RespawnPlayer(PhotonNetwork.LocalPlayer, robot);
            return;
        }

        Grabbable grab = other.GetComponent<Grabbable>();
        if(grab != null && grab.pView.IsMine)
        {
            grab.Respawn();
        }
    }
}
