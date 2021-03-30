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
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);
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
