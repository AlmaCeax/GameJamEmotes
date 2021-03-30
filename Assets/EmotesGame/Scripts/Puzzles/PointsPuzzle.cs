using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using Photon.Pun;
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
    private PlatformMove pMovement;
    public BGCcMath curve;
    public PhotonView pView;
    public float movementSpeed = 5.0f;

    private float distance = 0f;
    public bool Active { get { return active; } set { active = value; } }
    private bool active = false;

    private void Start()
    {
        pMovement = platform.GetComponent<PlatformMove>();
        SplineMovement(0.0f);
    }

    private void Update()
    {
        if (!pView.IsMine)
            return;

        switch (activationType)
        {
            case ActivationType.Automatic:
                break;
            case ActivationType.Pressure:
                {
                    SplineMovement(active ? movementSpeed * Time.deltaTime : -movementSpeed * Time.deltaTime);
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
    public void SplineMovement(float displacement)
    {
        distance = Mathf.Clamp(distance + displacement, 0, curve.GetDistance());
        
        //calculate position and tangent
        Vector3 tangent;
        Vector3 nextPosition = curve.CalcPositionAndTangentByDistance(distance, out tangent);
        pMovement.pView.RPC("Move", RpcTarget.All, nextPosition - platform.position);
        //pMovement.Move(nextPosition - platform.position);
        platform.rotation = Quaternion.LookRotation(tangent);
    }
}
