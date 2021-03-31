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
            if (!pView.Owner.IsLocal && !IsVisible())
                RepositionEmote();
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

    private bool IsVisible()
    {
        Rect cameraRect = Camera.main.rect;
        Rect emoteRect = GetComponent<Image>().rectTransform.rect;
        if ((emoteRect.x >= cameraRect.x && emoteRect.x + emoteRect.width <= cameraRect.x + cameraRect.width
            && emoteRect.y >= cameraRect.y && emoteRect.y + emoteRect.height <= cameraRect.y + cameraRect.height))
            return true;
        else
            return false;
    }

    private void RepositionEmote()
    {
        Rect cameraRect = Camera.main.rect;
        Rect emoteRect = GetComponent<Image>().rectTransform.rect;
        if (emoteRect.x < cameraRect.x)
            transform.position = new Vector3(cameraRect.x, transform.position.y, transform.position.z);
        else if(emoteRect.x + emoteRect.width > cameraRect.x + cameraRect.width)
            transform.position = new Vector3((cameraRect.x + cameraRect.width) - emoteRect.width, transform.position.y, transform.position.z);
        else if (emoteRect.y < cameraRect.y)
            transform.position = new Vector3(transform.position.x, cameraRect.y, transform.position.z);
        else if (emoteRect.y + emoteRect.height > cameraRect.y + cameraRect.height)
            transform.position = new Vector3(transform.position.x, (cameraRect.y + cameraRect.height) - emoteRect.height, transform.position.z);
    }

}
