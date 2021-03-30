using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //private CharacterController controller;
    private Rigidbody rbody;
    private Animator anim;
    private Robot player;
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    public float baseSpeed = 3.0f;
    public float grabbingSpeed = 1.5f;
    private float playerSpeed = 2.0f;
    public float playerMaxSpeed = 5.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    private float groundCheckerRadius = 0.1f;
    private int groundLayerMask;
    private Camera currentCamera;

    private Vector3 move;

    private void Start()
    {
        //controller = GetComponent<CharacterController>();
        rbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GetComponent<Robot>();
        currentCamera = Camera.main;
    }

    void Update()
    {
        move = Vector3.zero;

        if (!player.pView.IsMine)
        {
            return;
        }

        int groundLayerMask = 1 << 6;

        if (Physics.OverlapSphere(transform.position, groundCheckerRadius, groundLayerMask).Length > 0)
        {
            groundedPlayer = true;
            anim.SetBool("Fall", false);
        }
        else if (groundedPlayer)
            groundedPlayer = false; 

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float cameraVerticalRotation = currentCamera.transform.eulerAngles.x;
        Vector3 forwardCamera = Quaternion.AngleAxis(-cameraVerticalRotation, currentCamera.transform.right) * currentCamera.transform.forward;
        move = move.z * forwardCamera + move.x * currentCamera.transform.right;

        switch (player.state)
        {
            case Robot.STATE.NONE:
                playerSpeed = baseSpeed;
                break;
            case Robot.STATE.GRABBING:
                playerSpeed = grabbingSpeed;
                move = Vector3.Dot(transform.forward, move) * transform.forward;
                break;
            default:
                break;
        }

        //controller.Move(move * Time.deltaTime * playerSpeed);
        if (player.currentGrabbedItem)
            player.currentGrabbedItem.playerDirection = move * Time.deltaTime * playerSpeed;

        if (move != Vector3.zero && player.state == Robot.STATE.NONE)
        {
            gameObject.transform.forward = move;
            anim.SetFloat("Speed", move.magnitude);
        }
        else
        {
            anim.SetFloat("Speed", 0.0f);
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            anim.SetBool("Jump", true);
        }

        if(playerVelocity.y < 0)
        {
            if (anim.GetBool("Jump") == true)
            {
                anim.SetBool("Jump", false);
            }

            anim.SetBool("Fall", true);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        //controller.Move(playerVelocity * Time.deltaTime);
    }

    void FixedUpdate()
    {
        Debug.Log(move);

        if (rbody.velocity.magnitude <= playerMaxSpeed)
        {
            rbody.AddForce(move * playerSpeed, ForceMode.Acceleration);
            Debug.Log("Applying force");
        }

        if (move == Vector3.zero)
        {
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = Vector3.zero;
        }
    }
}
