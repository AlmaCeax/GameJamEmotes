using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsPuzzle : MonoBehaviour
{
    public Transform platform;
    public BGCcMath curve;
    public float movementSpeed = 5.0f;


    private float distance = 0f;
    public bool Active { get { return active; } set { active = value; } }
    private bool active = false;

    private void Update()
    {
        //setup distance
        distance = active? Mathf.Clamp(distance + movementSpeed * Time.deltaTime, distance, curve.GetDistance()) : Mathf.Clamp(distance - movementSpeed * Time.deltaTime, 0, distance);

        //calculate position and tangent
        Vector3 tangent;
        platform.position = curve.CalcPositionAndTangentByDistance(distance, out tangent);
        platform.rotation = Quaternion.LookRotation(tangent);
    }
}
