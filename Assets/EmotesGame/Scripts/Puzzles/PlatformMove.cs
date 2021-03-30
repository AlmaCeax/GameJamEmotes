using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour, IPunObservable
{
    // Start is called before the first frame update
    public Robot player1;
    public Robot player2;
    public PhotonView pView;

    private Vector3 lastPosition;

    public void LateUpdate()
    {
        /*if (!pView.IsMine)
        {
            if(lastPosition != transform.position)
            {
                if (player1 && player1.pView.IsMine)
                    player1.movement.controller.Move(transform.position - lastPosition);
                if (player2 && player2.pView.IsMine)
                    player2.movement.controller.Move(transform.position - lastPosition);
            }

            lastPosition = transform.position;
        }*/
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       /* if (stream.IsWriting)
        {
            stream.SendNext(player1 ? player1.pView.Owner : null);
            stream.SendNext(player2 ? player1.pView.Owner : null);
        }
        else
        {
            Photon.Realtime.Player pl1 = (Photon.Realtime.Player)stream.ReceiveNext();
            if(pl1 != null)
                player1 = GameManager.Instance.player1.pView.Owner == pl1 ? GameManager.Instance.player1 : GameManager.Instance.player2;

            Photon.Realtime.Player pl2 = (Photon.Realtime.Player)stream.ReceiveNext();
            if (pl2 != null)
                player2 = GameManager.Instance.player1.pView.Owner == pl2 ? GameManager.Instance.player1 : GameManager.Instance.player2;
        }*/
    }

}
