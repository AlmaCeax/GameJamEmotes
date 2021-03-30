using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpCubes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Grabbable")
        {
            if(collision.contacts[0].point.z > transform.position.z)
            {
                collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z - collision.gameObject.transform.localScale.z - transform.localScale.z);
            }else if (collision.contacts[0].point.z < transform.position.z)
            {
                collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z + collision.gameObject.transform.localScale.z + transform.localScale.z);
            }
        }
    }
}
