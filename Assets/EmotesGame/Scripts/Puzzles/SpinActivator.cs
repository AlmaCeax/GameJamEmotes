using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinActivator : MonoBehaviour
{
    public PointsPuzzle puzzle;
    public float spinForce = 5.0f;
    public PhotonView pView;

    GameObject grabber;

    [PunRPC]
    public void Spin(float force)
    {
        if (!pView.Owner.IsMasterClient)
            return;

        float displacement = force * Time.deltaTime;
        if(puzzle.CanMove(displacement))
        {
            transform.Rotate(transform.up, -displacement * 20);
            puzzle.SplineMovement(displacement);
        }
    }

    public void OnGrab(GameObject g)
    {
        grabber = g;
        //disable control etc
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            Spin(spinForce);
        else if (Input.GetKey(KeyCode.E))
            Spin(-spinForce);
    }
}
