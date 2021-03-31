using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Robot : MonoBehaviour
{
    public enum STATE { NONE, GRABBING};
    public STATE state = STATE.NONE;
    enum EMOTETYPE { YES, NO, HERE, JUMP, DANCE, NONE = -1 };

    private bool grabbing = false;
    public Grabbable currentGrabbedItem = null;
    public GameObject armControl;
    private float currentArmGrabValue = 0.0f;

    private Animator anim;
    public PhotonView pView;
    public PlayerMovement movement;

    private GameObject[] emotes;
    private GameObject emoteCanvas;
    internal static Robot LocalPlayerInstance;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        pView = GetComponent<PhotonView>();
        movement = GetComponent<PlayerMovement>();

        if (pView.IsMine)
            LocalPlayerInstance = this;
    }

    private void Start()
    {
        emotes = new GameObject[5];
        emoteCanvas = transform.GetChild(2).gameObject;
        for(int i = 0; i < 5; ++i)
        {
            emotes[i] = emoteCanvas.transform.GetChild(i).gameObject;
        }

        emoteCanvas.GetComponent<Canvas>().worldCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();
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
        StartCoroutine("GrabAnimation");
        state = STATE.GRABBING;
        anim.SetBool("Grab", true);
        grabbing = true;
        if(!currentGrabbedItem.pView.AmOwner)
            currentGrabbedItem.pView.TransferOwnership(PhotonNetwork.LocalPlayer);

        currentGrabbedItem.grabbed = true;
    }

    void ReleaseGrab()
    {
        StartCoroutine("UnGrabAnimation");
        anim.SetBool("Grab", false);
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

    IEnumerator GrabAnimation()
    {
        for (float f = currentArmGrabValue; f < 1.0; f += 0.1f)
        {
            armControl.GetComponent<Rig>().weight = f;
            currentArmGrabValue = armControl.GetComponent<Rig>().weight;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator UnGrabAnimation()
    {
        for (float f = currentArmGrabValue; f > 0.0; f -= 0.1f)
        {
            armControl.GetComponent<Rig>().weight = f;
            currentArmGrabValue = armControl.GetComponent<Rig>().weight;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnBecameInvisible()
    {
        foreach (GameObject e in emotes)
        {
            e.GetComponent<Emote>().isVisible = false;
            e.GetComponent<Emote>().exitedPosition = e.gameObject.transform.position;
        }
        Debug.Log("Invisible");
    }
    private void OnBecameVisible()
    {
        foreach (GameObject e in emotes)
        {
            e.GetComponent<Emote>().isVisible = true;
            e.GetComponent<Emote>().exitedPosition = Vector3.zero;
        }
        Debug.Log("Visible");
    }
}
