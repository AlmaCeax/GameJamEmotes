using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinActivator : MonoBehaviour
{
    public PointsPuzzle puzzle;
    public float spinForce = 5.0f;

    public void Spin(float force)
    {
        float displacement = force * Time.deltaTime;
        if(puzzle.CanMove(displacement))
        {
            transform.Rotate(transform.up, displacement * 20);
            puzzle.Move(displacement);
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            Spin(-spinForce);
        else if(Input.GetKey(KeyCode.E))
            Spin(spinForce);
    }
}
