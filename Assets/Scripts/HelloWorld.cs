using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HelloWorld : MonoBehaviour
{
    private NetworkManager networkManager;

    private void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!networkManager.IsClient && !networkManager.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            SubmitNewPosition();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) networkManager.StartHost();
        if (GUILayout.Button("Client")) networkManager.StartClient();
        if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
    }

    static void StatusLabels()
    {
        var mode = networkManager.IsHost ?
            "Host" : m_NetworkManager.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    static void SubmitNewPosition()
    {
        if (GUILayout.Button(m_NetworkManager.IsServer ? "Move" : "Request Position Change"))
        {
            if (m_NetworkManager.IsServer && !m_NetworkManager.IsClient)
            {
                foreach (ulong uid in m_NetworkManager.ConnectedClientsIds)
                    m_NetworkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
            }
            else
            {
                var playerObject = m_NetworkManager.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.Move();
            }
        }
    }
}
}
