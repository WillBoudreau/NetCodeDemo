using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PuckBehaviour : NetworkBehaviour
{
   
    public float friction = 0.98f;    
    public float maxSpeed = 10f;        
    public float forceMultiplier = 2f; 
    public Vector3 startPosition;
    public bool isMoving;
    private Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = true; 
        rb.drag = 0f;          
        rb.angularDrag = 0.5f;
        rb.isKinematic = false;
    }

    void Update()
    {
        rb.isKinematic = false;
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity *= friction;
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ApplyForceServerRpc(Vector3 direction)
    {
        rb.AddForce(direction * forceMultiplier, ForceMode.Impulse);
        ApplyForceClientRpc(direction);
    }
    [ClientRpc(RequireOwnership = false)]
    public void ApplyForceClientRpc(Vector3 direction)
    {
        if(IsOwner)
        {
            rb.AddForce(direction * forceMultiplier, ForceMode.Impulse);
        }
    }
    public void ApplyForce(Vector3 direction)
    {
        isMoving = true;
        if (IsServer)
        {
            ApplyForceServerRpc(direction);
        }
        else if(IsClient)
        {
            ApplyForceClientRpc(direction);
        }
    }
    public void ResetPuck()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
    }
}
