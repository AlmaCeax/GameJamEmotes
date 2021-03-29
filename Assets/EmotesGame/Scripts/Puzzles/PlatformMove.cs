using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 direction;
    public bool move;
    public CharacterController player1;
    public CharacterController player2;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            if (player1)
                player1.Move(direction * Time.deltaTime);
            if (player2)
                player2.Move(direction * Time.deltaTime);
        }
        
    }

    public void UpdatePlatformDirection(Vector3 other_direction)
    {
        direction = other_direction;
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
