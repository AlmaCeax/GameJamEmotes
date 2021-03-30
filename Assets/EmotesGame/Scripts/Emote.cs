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

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if(pView.Owner.IsMasterClient)
                transform.LookAt(Camera.main.transform.position, Vector3.up);
            else
                transform.LookAt(Camera.main.transform.position + (transform.position - Camera.main.transform.position), Vector3.up);
        }
    }

    [PunRPC]
    public void Show()
    {
        isActive = true;
        gameObject.SetActive(true);
        Invoke("Hide", activeTime);
    }

    public void Hide()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

}
