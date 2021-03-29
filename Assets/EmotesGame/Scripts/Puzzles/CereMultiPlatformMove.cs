using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CereMultiPlatformMove : MonoBehaviour
{
    // Start is called before the first frame update
    private bool move;
    private bool left;
    public GameObject multiplatform;
    public int length;
    private int count;

    void Start()
    {
        move = false;
        left = false;
        count = length;
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            if(left && count > 0)
            {
                multiplatform.transform.position = new Vector3(multiplatform.transform.position.x + (1 * Time.deltaTime), multiplatform.transform.position.y, multiplatform.transform.position.z);
                count--;
            }
            else if(!left && count < length)
            {
                multiplatform.transform.position = new Vector3(multiplatform.transform.position.x - (1 * Time.deltaTime), multiplatform.transform.position.y, multiplatform.transform.position.z);
                count++;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        move = true;
        left = !left;
    }

    
}
