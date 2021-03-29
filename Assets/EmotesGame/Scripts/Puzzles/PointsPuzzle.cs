using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsPuzzle : MonoBehaviour
{
    public enum ActivationType
    {
        Automatic,
        Pressure,
        Manual
    }

    public ActivationType activationType;

    public Transform platform;
    public BGCcMath curve;
    public float movementSpeed = 5.0f;

    private float distance = 0f;
    public bool Active { get { return active; } set { active = value; } }
    private bool active = false;

    private void Update()
    {
        switch (activationType)
        {
            case ActivationType.Automatic:
                break;
            case ActivationType.Pressure:
                {
                    //setup distance
                    distance = active ? Mathf.Clamp(distance + movementSpeed * Time.deltaTime, distance, curve.GetDistance()) : Mathf.Clamp(distance - movementSpeed * Time.deltaTime, 0, distance);

                    //calculate position and tangent
                    Vector3 tangent;
                    platform.position = curve.CalcPositionAndTangentByDistance(distance, out tangent);
                    platform.rotation = Quaternion.LookRotation(tangent);
                }
                break;
            case ActivationType.Manual:
                break;
            default:
                break;
        }

    }

    public bool CanMove(float displacement)
    {
        float newDistance = Mathf.Clamp(distance + displacement, 0, curve.GetDistance());
        return newDistance < curve.GetDistance() && newDistance > 0;
    }
    public void Move(float displacement)
    {
        distance = Mathf.Clamp(distance + displacement, 0, curve.GetDistance());
        //calculate position and tangent
        Vector3 tangent;
        platform.position = curve.CalcPositionAndTangentByDistance(distance, out tangent);
        platform.rotation = Quaternion.LookRotation(tangent);
    }
}
