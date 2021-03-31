using Photon.Pun;
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
    private Robot player1;
    private Robot player2;

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
                if(player1 && player1.pView.IsMine)
                    player1.Move(-transform.right * Time.deltaTime);
                if(player2 && player1.pView.IsMine)
                    player2.Move(-transform.right * Time.deltaTime);
                count--;
            }
            else if(!left && count < length)
            {
                transform.position = new Vector3(transform.position.x + (1 * Time.deltaTime), transform.position.y, transform.position.z);
                if(player1 && player1.pView.IsMine)
                    player1.Move(transform.right * Time.deltaTime);
                if(player2 && player2.pView.IsMine)
                    player2.Move(transform.right * Time.deltaTime);

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
            return;
            
        if (player1 && player2)
        {
            move = true;
            left = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Robot>() == player1)
            player1 = null;
        if (other.GetComponent<Robot>() == player2)
            player2 = null;

        if (!pView.IsMine)
            return;
        
        if (!player1 || !player2)
        {
            move = true;
            left = false;
        }
    }
}
