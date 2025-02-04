using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            transform.position = spawnPosition;
        }
    }
    private void Update()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        if(!IsOwner)
        {
            return;
        }
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);

        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void OnTriggerEnter()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
            foreach(Collider hitCollider in hitColliders)
            {
                if(hitCollider.gameObject.tag == "Box")
                {
                    hitCollider.gameObject.transform.SetParent(this.transform, true);
                }
            }
        }
    }
}
