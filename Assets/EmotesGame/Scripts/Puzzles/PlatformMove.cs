using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 direction;
    public CharacterController player1;
    public CharacterController player2;
    public PhotonView pView;

    // Update is called once per frame
    void Update()
    {
        if (!pView.Owner.IsMasterClient)
            return;

        if (player1)
            player1.Move(direction);
        if (player2)
            player2.Move(direction);      
    }

    private void OnTriggerEnter(Collider other)
    {
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
    }



}
