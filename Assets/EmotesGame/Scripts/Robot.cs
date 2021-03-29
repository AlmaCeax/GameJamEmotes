using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public enum STATE { NONE, GRABBING};
    public STATE state = STATE.NONE;

    private float grabRadius = 0.5f;
    private bool grabbing = false;
    private Grabbable currentGrabbedItem = null;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float grabAxis = Input.GetAxis("Grab");
        if (grabAxis > 0.1f && !grabbing)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position + new Vector3(0, 0.5f, 0), grabRadius);

            foreach (Collider col in cols)
            {
                if (col.gameObject.tag == "Grabbable")
                {
                    currentGrabbedItem = col.gameObject.GetComponent<Grabbable>();
                    Grab();
                }
            }
        }
        else if(grabbing)
            ReleaseGrab();
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
    }
}
