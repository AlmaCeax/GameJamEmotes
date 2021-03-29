using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinActivator : MonoBehaviour
{
    public PointsPuzzle puzzle;
    public float spinForce = 5.0f;

    GameObject grabber;

    public void Spin(float force)
    {
        float displacement = force * Time.deltaTime;
        if(puzzle.CanMove(displacement))
        {
            transform.Rotate(transform.up, -displacement * 20);
            puzzle.Move(displacement);
        }
    }

    public void OnGrab(GameObject g)
    {
        grabber = g;
        //disable control etc
    }
    public void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Spin(horizontalInput * spinForce);
    }
}
