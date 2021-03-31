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
    public bool wasVisible = true;
    public Vector3 exitedPosition;
    public Vector3 cameraExitedPosition;
    private GameObject playerMesh;

    private void Start()
    {
        playerMesh = transform.parent.parent.GetChild(1).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (pView.Owner.IsLocal)
            transform.LookAt(Camera.main.transform);
        else
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);

            Plane[] cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            Bounds bounds = playerMesh.GetComponent<Renderer>().bounds;
            bool isVisible = GeometryUtility.TestPlanesAABB(cameraFrustumPlanes, bounds);

            if (wasVisible && !isVisible)
            {
                exitedPosition = transform.position;
                cameraExitedPosition = Camera.main.transform.position;
                wasVisible = false;
            }

            if (!wasVisible && isVisible)
            {
                transform.localPosition = Vector3.zero;
                wasVisible = true;
            }

            if (!isVisible)
                RepositionEmote();
            
        }
    }

    [PunRPC]
    public void Show()
    {
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
        if(exitedPosition.x > Camera.main.transform.position.x)
            transform.position = exitedPosition + (Camera.main.transform.position - cameraExitedPosition) - new Vector3(2, 0, 0);
        if (exitedPosition.x < Camera.main.transform.position.x)
            transform.position = exitedPosition + (Camera.main.transform.position - cameraExitedPosition) + new Vector3(2, 0, 0);
    }
}
