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
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);

            if (!pView.Owner.IsLocal && !isVisible)
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

    private void RepositionEmote()
    {
        transform.position = exitedPosition;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        exitedPosition = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }
}
