using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class NetTrigger : NetworkBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI scoreTextHome;
    public TextMeshProUGUI scoreTextAway;
    public PuckBehaviour puck;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        GameObject homeScoreObject = GameObject.Find("HomeScore");
        if (homeScoreObject != null)
        {
            scoreTextHome = homeScoreObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("HomeScore GameObject not found");
        }
        GameObject awayScoreObject = GameObject.Find("AwayScore");
        if (awayScoreObject != null)
        {
            scoreTextAway = awayScoreObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("AwayScore GameObject not found");
        }
    }

    void Start()
    {
        scoreTextHome.text = "Home: " + gameManager.homeScore.Value.ToString();
        scoreTextAway.text = "Away: " + gameManager.awayScore.Value.ToString();

        OnNetworkSpawn();
    }
    void Update()
    {
        GameObject homeScoreObject = GameObject.Find("HomeScore");
        GameObject awayScoreObject = GameObject.Find("AwayScore");
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        gameManager.homeScore.OnValueChanged += OnHomeScoreChanged;
        gameManager.awayScore.OnValueChanged += OnAwayScoreChanged;

        OnHomeScoreChanged(0, gameManager.homeScore.Value);
        OnAwayScoreChanged(0, gameManager.awayScore.Value);

        puck = GameObject.FindWithTag("Puck").GetComponent<PuckBehaviour>();
        GameObject puckObject = GameObject.FindWithTag("Puck");
        if (puckObject != null)
        {
            puck = puckObject.GetComponent<PuckBehaviour>();
        }
        else
        {
            Debug.LogError("Puck GameObject not found");
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puck" && this.gameObject.tag == "HomeNet")
        {
            UpdateAwayScoreServerRpc();
        }
        else if (other.gameObject.tag == "Puck" && this.gameObject.tag == "AwayNet")
        {
            UpdateHomeScoreServerRpc();
        }
        puck.ResetPuck();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateHomeScoreServerRpc()
    {
        gameManager.homeScore.Value++;
        UpdateHomeScoreClientRpc(gameManager.homeScore.Value);
        Debug.Log("Home Score: " + gameManager.homeScore.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateAwayScoreServerRpc()
    {
        gameManager.awayScore.Value++;
        UpdateAwayScoreClientRpc(gameManager.awayScore.Value);
        Debug.Log("Away Score: " + gameManager.awayScore.Value);
    }
    [ClientRpc]
    void UpdateHomeScoreClientRpc(int newValue)
    {
        gameManager.homeScore.Value = newValue;
        Debug.Log("Home Score: " + gameManager.homeScore.Value);
    }
    [ClientRpc]
    void UpdateAwayScoreClientRpc(int newValue)
    {
        gameManager.awayScore.Value = newValue;
        Debug.Log("Away Score: " + gameManager.awayScore.Value);
    }

    private void OnHomeScoreChanged(int oldValue, int newValue)
    {
        scoreTextHome.text = $"Home: {newValue}";
    }
    private void OnAwayScoreChanged(int oldValue, int newValue)
    {
        scoreTextAway.text = $"Away: {newValue}";
    }
}
