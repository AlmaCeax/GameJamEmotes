using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Robot player1;
    public Robot player2;
    public PhotonView pView;

    [PunRPC]
    public void Move(Vector3 movementVector)
    {
        transform.position += movementVector;

        if (player1 && player1.pView.IsMine)
            player1.movement.controller.Move(movementVector);
        if (player2 && player2.pView.IsMine)
            player2.movement.controller.Move(movementVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        Robot collidedRobot = other.GetComponent<Robot>();

        if (player1 == null)
            player1 = collidedRobot;
        else if(collidedRobot != player1)
            player2 = collidedRobot;
    }

    private void OnTriggerExit(Collider other)
    {
        Robot collidedRobot = other.GetComponent<Robot>();

        if (collidedRobot == player1)
            player1 = null;
        if (collidedRobot == player2)
            player2 = null;
    }
}
