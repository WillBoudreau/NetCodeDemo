using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using System.Net.Sockets;
using System.Net;
using Unity.Netcode.Transports.UTP;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI playersCountText;
    [SerializeField] TextMeshProUGUI ipAddressText;
    [SerializeField] TMP_InputField ip;

    [SerializeField] string ipAddress;
    [SerializeField] UnityTransport transport;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone);

    void Awake()
    {
        //ipAddress = "0.0.0.0";
        if (IsClient || IsServer)
        {
            NetworkManager.Shutdown();
        }
        hostButton.onClick.AddListener(() => 
        {
            GetLocalIPAddress();
            NetworkManager.Singleton.StartHost();
        });
        clientButton.onClick.AddListener(() => 
        {
            SetIpAddress();
            NetworkManager.Singleton.StartClient();
        });
    }

    void Update()
    {
        playersCountText.text = $"Players: {playersCount.Value}";
        if(!IsServer)
        {
            return;
        }
        playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
    /* Gets the Ip Address of your connected network and
	shows on the screen in order to let other players join
	by inputing that Ip in the input field */
    // ONLY FOR HOST SIDE 
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressText.text = ip.ToString();
                ipAddress = ip.ToString();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    /* Sets the Ip Address of the Connection Data in Unity Transport
	to the Ip Address which was input in the Input Field */
    // ONLY FOR CLIENT SIDE
    public void SetIpAddress()
    {
        ipAddress = ip.text;
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
        Debug.Log(transport.ConnectionData.Address);
        Debug.Log(ipAddress);
    }

}
