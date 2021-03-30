using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emote : MonoBehaviour
{
    public Image emoteImage;
    public bool isActive = false;
    public float activeTime = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
            transform.LookAt(Camera.main.transform, Vector3.up);
    }

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
