using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public bool grabbed = false;
    public Vector3 playerDirection;
    public PhotonView pView;
    // Start is called before the first frame update
    void Start()
    {
        pView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed)
        {
            transform.position += playerDirection;
        }
    }
}

