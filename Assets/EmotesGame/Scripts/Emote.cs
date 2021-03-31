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
    Vector3 exitedPosition;

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if(pView.Owner.IsLocal)
                transform.LookAt(Camera.main.transform);
            else
            {
                transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);
                if (!isVisible)
                    RepositionEmote();
            }
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
        transform.position = exitedPosition;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        exitedPosition = transform.position;
    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }
}
