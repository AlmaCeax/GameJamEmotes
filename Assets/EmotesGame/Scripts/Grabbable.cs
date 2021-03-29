using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public bool grabbed = false;
    public Vector3 playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ParentToPlayer(GameObject player)
    {
        transform.SetParent(player.transform);
    }

    public void UnParentToPlayer()
    {
        transform.SetParent(null);
    }
}

