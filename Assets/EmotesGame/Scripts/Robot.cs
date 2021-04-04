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

    public Emote[] emotes;
    private GameObject emoteCanvas;
    public AudioClip[] emoteClips;

    internal static Robot LocalPlayerInstance;

    private AudioSource asource;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        pView = GetComponent<PhotonView>();
        movement = GetComponent<PlayerMovement>();
        asource = GetComponent<AudioSource>();

        if(GameManager.Instance)
        {
            if (GameManager.Instance.player1 == null)
                GameManager.Instance.player1 = this;
            else if(GameManager.Instance.player2 == null)
                GameManager.Instance.player2 = this;
        }

        if (pView.IsMine)
            LocalPlayerInstance = this;
    }

    private void Start()
    {
        emotes = new Emote[5];
        emoteCanvas = transform.GetChild(2).gameObject;
        for(int i = 0; i < 5; ++i)
        {
            emotes[i] = emoteCanvas.transform.GetChild(i).gameObject.GetComponent<Emote>();
            emotes[i].enabled = true;
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

        if (Input.anyKeyDown)
            anim.SetBool("Dancing", false);

        CheckGrabbables();
        EmoteInputs();
    }

    [PunRPC]
    public void HideActiveEmotes()
    {
        foreach (Emote e in emotes)
        {
            if (e.isActive)
            {
                e.Hide();
            }
        }
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
        if (grabAxis > 0.1f || Input.GetKeyDown(KeyCode.E) && !grabbing)
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
        
        if (grabbing && (grabAxis > 0.1f || Input.GetKey(KeyCode.E)))
        {
            RaycastHit hit;

            if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out hit, 0.5f))
                ReleaseGrab();
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out hit, 0.5f))
                    if (hit.collider.gameObject.tag != "Grabbable")
                        ReleaseGrab();

        }
        else if(grabbing)
           ReleaseGrab();
    }

    void EmoteInputs()
    {
        Vector2 arrows = new Vector2(Input.GetAxis("HorizontalArrow"), Input.GetAxis("VerticalArrow"));

        if (arrows.magnitude >= 0.1f)
        {
            pView.RPC("HideActiveEmotes", RpcTarget.All);

            if (arrows.x > 0.1f)
            {
                emotes[(int)EMOTETYPE.NO].pView.RPC("Show", RpcTarget.All);
                pView.RPC("PlayRPCEmoteSFX", RpcTarget.All, (int)EMOTETYPE.NO);
            }
            else if (arrows.x < -0.1f)
            {
                emotes[(int)EMOTETYPE.YES].pView.RPC("Show", RpcTarget.All);
                pView.RPC("PlayRPCEmoteSFX", RpcTarget.All, (int)EMOTETYPE.YES);
            }
            else if (arrows.y < -0.1f)
            {
                emotes[(int)EMOTETYPE.HERE].pView.RPC("Show", RpcTarget.All);
                pView.RPC("PlayRPCEmoteSFX", RpcTarget.All, (int)EMOTETYPE.HERE);
            }
            else if (arrows.y > 0.1f)
            {
                emotes[(int)EMOTETYPE.JUMP].pView.RPC("Show", RpcTarget.All);
                pView.RPC("PlayRPCEmoteSFX", RpcTarget.All, (int)EMOTETYPE.JUMP);
            }
   
        }

        if (Input.GetButtonDown("LeftBump") || Input.GetKeyDown(KeyCode.Alpha5))
        {
            pView.RPC("HideActiveEmotes", RpcTarget.All);

            emotes[(int)EMOTETYPE.DANCE].pView.RPC("Show", RpcTarget.All);

            anim.SetBool("Dancing", true);
        }
    }

    [PunRPC]
    public void PlayRPCEmoteSFX(int clip)
    {
        if (!asource.isPlaying)
            asource.PlayOneShot(emoteClips[clip]);
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
}
