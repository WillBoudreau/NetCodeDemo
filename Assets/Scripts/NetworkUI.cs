using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI playersCountText;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone);

    void Awake()
    {
        hostButton.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartHost();
        });
        clientButton.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    void Update()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            var connectedClients = NetworkManager.Singleton.ConnectedClients;
            Debug.Log("Number of Connected Clients " + connectedClients.Count);
        }
        else
        {
            Debug.Log("This function can only be accessed on the server");
        }
        playersCountText.text = $"Players: {playersCount.Value}";
        if(!IsServer)
        {
            return;
        }
        playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
