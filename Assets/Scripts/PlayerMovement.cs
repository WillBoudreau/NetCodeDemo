using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Class Calls")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;
    [Header("Player Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject puck;
    [SerializeField] private float PlayerMinDistPuck;
    [SerializeField] private bool isPaused;
    [SerializeField] private Rigidbody rb;
    private PuckBehaviour puckScript;

    public override void OnNetworkSpawn()
    {
        isPaused = false;
        rb = GetComponent<Rigidbody>();
        if (IsOwner)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            transform.position = spawnPosition;
        }
        this.GetComponent<NetworkObject>().SpawnWithOwnership(clientId: NetworkManager.Singleton.LocalClientId);
    }

    private void Update()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        puck = GameObject.FindGameObjectWithTag("Puck");
        puckScript = GameObject.FindObjectOfType<PuckBehaviour>();
        rb.isKinematic = false;
        MovePlayer();
        Shoot();
        PlayerInputs();
    }

    void MovePlayer()
    {
        if (!IsOwner)
        {
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");
            other.transform.SetParent(this.transform, true);
        }
    }
    public void Shoot()
    {
        if (Vector3.Distance(transform.position, puck.transform.position) < PlayerMinDistPuck)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ApplyForceServerRpc(transform.forward);
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ApplyForceServerRpc(Vector3 direction)
    {
        puckScript.ApplyForce(direction);
        ApplyForceClientRpc(direction);
    }
    [ClientRpc(RequireOwnership = false)]
    public void ApplyForceClientRpc(Vector3 direction)
    {
        if (IsOwner)
        {
            puckScript.ApplyForce(direction);
        }
    }
    void PlayerInputs()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            gameManager.PauseGame();
            uiManager.LoadUI("PauseMenuUI");
            isPaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            gameManager.ResumeGame();
            uiManager.LoadUI("GameUI");
            isPaused = false;
        }
    }

}
