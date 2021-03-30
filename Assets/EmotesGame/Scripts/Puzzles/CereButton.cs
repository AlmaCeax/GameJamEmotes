using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CereButton : MonoBehaviour
{
    public bool pressed;
    public bool up;
    public int offset;
    public float originalstate;
    // Start is called before the first frame update
    void Start()
    {
        pressed = false;
        up = false;
        offset = 0;
        originalstate = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(pressed)
        {
            transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y - (0.5f * Time.deltaTime), transform.parent.position.z);
            if (transform.parent.position.y < offset)
            {
                pressed = false;
                up = true;
            }
        }
        else if (up)
        {
            transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y + (0.5f * Time.deltaTime), transform.parent.position.z);
            if (transform.parent.position.y > originalstate)
            {
                up = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pressed && !up)
        {
            pressed = true;
            transform.parent.GetComponentInParent<CereButtonsPuzzle>().CheckUpdatePressed(gameObject);
        }
    }
}
