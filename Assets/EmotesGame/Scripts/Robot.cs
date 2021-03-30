using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public enum STATE { NONE, GRABBING};
    public STATE state = STATE.NONE;

    private float grabRadius = 0.5f;
    private bool grabbing = false;
    public Grabbable currentGrabbedItem = null;

    private Animator anim;
    public PhotonView pView;
    public PlayerMovement movement;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        pView = GetComponent<PhotonView>();
        movement = GetComponent<PlayerMovement>();

        if (!GameManager.Instance.player1)
            GameManager.Instance.player1 = this;
        else if (!GameManager.Instance.player2)
            GameManager.Instance.player2 = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pView.IsMine)
        {
            return;
        }

        float grabAxis = Input.GetAxis("Grab");
        if (grabAxis > 0.1f && !grabbing)
        {
            RaycastHit hit;

            if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out hit,  0.5f))
            {
                if(hit.collider.gameObject.tag == "Grabbable")
                {
                    transform.forward = -hit.normal;
                    currentGrabbedItem = hit.collider.gameObject.GetComponent<Grabbable>();
                    Grab();
                }

            }
        }
        else if(grabAxis < 0.1f && grabbing)
            ReleaseGrab();

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, Color.red);
    }

    void Grab()
    {
        state = STATE.GRABBING;
        grabbing = true;
        currentGrabbedItem.grabbed = true;
    }

    void ReleaseGrab()
    {
        state = STATE.NONE;
        grabbing = false;
        currentGrabbedItem.grabbed = false;
        currentGrabbedItem = null;
    }
}
