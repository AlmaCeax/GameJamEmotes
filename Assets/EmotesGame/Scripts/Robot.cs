using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public enum STATE { NONE, GRABBING};
    public STATE state = STATE.NONE;
    enum EMOTETYPE { YES, NO, HERE, JUMP, DANCE, NONE = -1 };

    private bool grabbing = false;
    public Grabbable currentGrabbedItem = null;

    private Animator anim;
    public PhotonView pView;
    public PlayerMovement movement;

    private GameObject[] emotes;
    private GameObject emoteCanvas;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        pView = GetComponent<PhotonView>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        emotes = new GameObject[5];
        emoteCanvas = transform.GetChild(2).gameObject;
        for(int i = 0; i < 5; ++i)
        {
            emotes[i] = emoteCanvas.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pView.IsMine)
        {
            return;
        }

        CheckGrabbables();
        EmoteInputs();
    }

    bool CheckIsEmoteActive()
    {
        foreach (GameObject e in emotes)
        {
            if (e.GetComponent<Emote>().isActive)
                return true;
        }
        return false;
    }

    void Grab()
    {
        state = STATE.GRABBING;
        grabbing = true;
        if(!currentGrabbedItem.pView.AmOwner)
            currentGrabbedItem.pView.TransferOwnership(PhotonNetwork.LocalPlayer);

        currentGrabbedItem.grabbed = true;
    }

    void ReleaseGrab()
    {
        state = STATE.NONE;
        grabbing = false;
        currentGrabbedItem.grabbed = false;
        currentGrabbedItem = null;
    }

    void CheckGrabbables()
    {
        float grabAxis = Input.GetAxis("Grab");
        if (grabAxis > 0.1f && !grabbing)
        {
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out hit, 0.5f))
            {
                if (hit.collider.gameObject.tag == "Grabbable")
                {
                    transform.forward = -hit.normal;
                    currentGrabbedItem = hit.collider.gameObject.GetComponent<Grabbable>();
                    Grab();
                }

            }
        }
        
        if (grabbing && grabAxis > 0.1f)
        {
            RaycastHit hit;

            if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out hit, 0.5f))
                ReleaseGrab();

        }
        else if(grabbing)
           ReleaseGrab();
    }

    void EmoteInputs()
    {
        Vector2 arrows = new Vector2(Input.GetAxis("HorizontalArrow"), Input.GetAxis("VerticalArrow"));

        if(!CheckIsEmoteActive())
        if (arrows.x > 0.1f)
            emotes[(int)EMOTETYPE.YES].GetComponent<Emote>().pView.RPC("Show", RpcTarget.All);
        else if (arrows.x < -0.1f)
            emotes[(int)EMOTETYPE.NO].GetComponent<Emote>().pView.RPC("Show", RpcTarget.All);
        else if (arrows.y < -0.1f)
            emotes[(int)EMOTETYPE.HERE].GetComponent<Emote>().pView.RPC("Show", RpcTarget.All);
        else if (arrows.y > 0.1f)
            emotes[(int)EMOTETYPE.JUMP].GetComponent<Emote>().pView.RPC("Show", RpcTarget.All);
    }
}
