using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;//The player's name
    [SerializeField] private NetworkUI networkUI;//The network UI

    public override void OnNetworkSpawn()
    {
        networkUI = FindObjectOfType<NetworkUI>();//Find the network UI
        //If the player is the local player
        if (IsLocalPlayer)
        {
            //Set the player's name to the name input field
            playerName.text = networkUI.playerName.text;
        }
    }
    [ServerRpc]
    public void UpdateNameTagServerRPC()
    {

    }
    [ClientRpc]
    public void UpdateNameTagClientRPC()
    {

    }
}
