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
    [Header("Class calls")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI playersCountText;
    public TMP_InputField playerName;
    [Header("IP Address")]
    [SerializeField] TextMeshProUGUI ipAddressText;
    [SerializeField] TMP_InputField ip;
    [SerializeField] string ipAddress;
    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] UnityTransport transport;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        uiManager = FindObjectOfType<UIManager>();
        errorText.text = "";
        if (IsClient || IsServer || IsHost)
        {
            NetworkManager.Shutdown();
        }
    }
    /// <summary>
    /// Start the host
    /// </summary>
    public void StartHost()
    {
        GetLocalIPAddress();
        NetworkManager.Singleton.StartHost();
    }
    /// <summary>
    /// Start the Client
    /// </summary>
    public void StartClient()
    {
        SetIpAddress();
        NetworkManager.Singleton.StartClient();
    }

    void Update()
    {
        playersCountText.text = $"Players: {playersCount.Value}";
        if (IsServer)
        {
            playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
            UpdatePlayersCountServerRPC(playersCount.Value);
        }
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
                ipAddressText.text = "IP: " + ip.ToString();
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
        if (CheckIPAddress())
        {
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = ipAddress;
            ipAddressText.text = "IP: " + ipAddress;
            uiManager.LoadUI("GameUI");
        }
        else if (CheckIPAddress() == false)
        {
            uiManager.LoadUI("EnterIPUI");
            return;
        }
        Debug.Log(transport.ConnectionData.Address);
        Debug.Log(ipAddress);
    }
    bool CheckIPAddress()
    {
        if (string.IsNullOrEmpty(ipAddress) || !IPAddress.TryParse(ipAddress, out _))
        {
            errorText.text = "Please enter a valid IP Address";
            return false;
        }
        else
        {
            errorText.text = "";
            return true;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayersCountServerRPC(int count)
    {
        playersCount.Value = count;
        UpdatePlayersCountClientRPC(playersCount.Value);
    }
    [ClientRpc]
    public void UpdatePlayersCountClientRPC(int count)
    {
        playersCount.Value = count;
    }
}
