using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Robot player1;
    public Robot player2;
    public PhotonView pView;
    private Vector3 lastPosition;

    public void LateUpdate()
    {
        if(!pView.IsMine)
        {
            if (lastPosition != transform.position)
            {
                if (player1 && player1.pView.IsMine)
                    player1.movement.controller.Move(transform.position - lastPosition);
                if (player2 && player2.pView.IsMine)
                    player2.movement.controller.Move(transform.position - lastPosition);
            }

            lastPosition = transform.position;
        }
    }

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
