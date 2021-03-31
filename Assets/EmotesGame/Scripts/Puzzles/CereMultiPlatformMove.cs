using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CereMultiPlatformMove : MonoBehaviour
{
    // Start is called before the first frame update
    private bool move;
    private bool left;
    public int length;
    private int count;
    public Robot player1 = null;
    public Robot player2 = null;

    private PhotonView pView;
    private Vector3 lastPosition;

    void Start()
    {
        move = false;
        left = true;
        count = length;
        pView = GetComponent<PhotonView>();
    }

    public void LateUpdate()
    {
        if (!pView.IsMine)
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

    // Update is called once per frame
    void Update()
    {
        if (!pView.IsMine)
            return;

        if(move)
        {
            if(left && count > 0)
            {       
                transform.position = new Vector3(transform.position.x - (1 * Time.deltaTime), transform.position.y, transform.position.z);
                if(player1 != null && player1.pView.IsMine)
                    player1.movement.controller.Move(-transform.right * Time.deltaTime);
                if(player2 != null && player2.pView.IsMine)
                    player2.movement.controller.Move(-transform.right * Time.deltaTime);
                count--;
            }
            else if(!left && count < length)
            {
                transform.position = new Vector3(transform.position.x + (1 * Time.deltaTime), transform.position.y, transform.position.z);
                if(player1 != null && player1.pView.IsMine)
                    player1.movement.controller.Move(transform.right * Time.deltaTime);
                if(player2 != null && player2.pView.IsMine)
                    player2.movement.controller.Move(transform.right * Time.deltaTime);

                count++;
            }
            if (count == 0)
                left = true;
            else if (count == length)
                left = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player1 == null)
            player1 = other.GetComponent<Robot>();
        else if(other.GetComponent<Robot>() != player1)
            player2 = other.GetComponent<Robot>();
        
        if (!pView.IsMine)
        {
            pView.RPC("OnPlatformEnter", RpcTarget.Others, PhotonNetwork.LocalPlayer);
            return;
        }
          
        if (player1 && player2)
        {
            move = true;
            left = true;
        }
    }

    [PunRPC]
    public void OnPlatformEnter(Player player)
    {
        if (player1 == null)
            player1 = player.ActorNumber == 1 ? GameManager.Instance.player1 : GameManager.Instance.player2;
        else if (player2 == null)
            player2 = player.ActorNumber == 1 ? GameManager.Instance.player1 : GameManager.Instance.player2;


        if (player1 && player2)
        {
            move = true;
            left = true;
        }
    }

    [PunRPC]
    public void OnPlatformExit(Player player)
    {
        Robot exitRobot = player.ActorNumber == 1 ? GameManager.Instance.player1 : GameManager.Instance.player2;
        if (player1 == exitRobot)
            player1 = null;
        else if (player2 == exitRobot)
            player2 = null;

        if (!player1 || !player2)
        {
            move = true;
            left = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Robot>() == player1)
            player1 = null;
        if (other.GetComponent<Robot>() == player2)
            player2 = null;

        if (!pView.IsMine)
        {
            pView.RPC("OnPlatformExit", RpcTarget.Others, PhotonNetwork.LocalPlayer);
            return;
        }

        if (!player1 || !player2)
        {
            move = true;
            left = false;
        }
    }
}
