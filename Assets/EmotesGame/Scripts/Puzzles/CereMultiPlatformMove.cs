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
    private CharacterController player1;
    private CharacterController player2;

    void Start()
    {
        move = false;
        left = true;
        count = length;
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            if(left && count > 0)
            {
                transform.position = new Vector3(transform.position.x + (1 * Time.deltaTime), transform.position.y, transform.position.z);
                if(player1)
                    player1.Move(transform.right * Time.deltaTime);
                if(player2)
                    player2.Move(transform.right * Time.deltaTime);
                count--;
            }
            else if(!left && count < length)
            {
                transform.position = new Vector3(transform.position.x - (1 * Time.deltaTime), transform.position.y, transform.position.z);
                if(player1)
                    player1.Move(-transform.right * Time.deltaTime);
                if(player2)
                    player2.Move(-transform.right * Time.deltaTime);
                count++;
            }
            if (count == length)
                left = true;
            else if (count == 0)
                left = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        move = true;
        if (player1 == null)
            player1 = other.GetComponent<CharacterController>();
        else if(other.GetComponent<CharacterController>() != player1)
            player2 = other.GetComponent<CharacterController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() == player1)
            player1 = null;
        if (other.GetComponent<CharacterController>() == player2)
            player2 = null;
        
        if (!player1 && !player2)
            move = false;
    }



}
