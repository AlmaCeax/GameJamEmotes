using Photon.Pun;
using Photon.Pun.Simple;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public bool grabbed = false;
    public Vector3 playerDirection;
    public Vector3 startPosition;
    public PhotonView pView;
    // Start is called before the first frame update
    void Start()
    {
        pView = GetComponent<PhotonView>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed)
        {
            transform.position += playerDirection;
        }
    }

    public void Respawn()
    {
        GetComponent<SyncTransform>().FlagTeleport();
        transform.position = startPosition;
    }
}

