using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emote : MonoBehaviour
{
    public Image emoteImage;
    public bool isActive = false;
    public float activeTime = 3.0f;
    public PhotonView pView;
    public bool isVisible = true;
    public Vector3 exitedPosition;
    public Vector3 originalPosition;
    private GameObject playerMesh;

    private void Start()
    {
        originalPosition = transform.position;
        playerMesh = transform.parent.parent.GetChild(1).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        Plane[] cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        Bounds bounds = playerMesh.GetComponent<Renderer>().bounds;
        bool inBounds = GeometryUtility.TestPlanesAABB(cameraFrustumPlanes, bounds);
        if(pView.Owner.IsLocal)
            transform.LookAt(Camera.main.transform);
        else
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);
            if (!isVisible)
                RepositionEmote();
            
        }
    }

    [PunRPC]
    public void Show()
    {
        enabled = true;
        isActive = true;
        GetComponent<Image>().color = Color.white;
        Invoke("Hide", activeTime);
    }

    public void Hide()
    {
        isActive = false;
        GetComponent<Image>().color = Color.clear;
    }

    private void RepositionEmote()
    {
        transform.position = Camera.main.transform.position + exitedPosition;
        Debug.Log("Entro en reposition");
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        exitedPosition = transform.position;
        Debug.Log("Invisible");
    }
    private void OnBecameVisible()
    {
        isVisible = true;
        Debug.Log("Visible");
    }
}
