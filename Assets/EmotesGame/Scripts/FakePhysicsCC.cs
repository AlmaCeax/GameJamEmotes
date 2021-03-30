using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePhysicsCC : MonoBehaviour
{
    public float characterWeight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        body.AddForceAtPosition(Vector3.down * characterWeight * this.GetComponent<CharacterController>().velocity.magnitude, transform.position);
    }
}
