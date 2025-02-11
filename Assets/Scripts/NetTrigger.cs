using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class NetTrigger : NetworkBehaviour
{
    public TextMeshProUGUI scoreTextHome;
    public TextMeshProUGUI scoreTextAway;
    public AudioSource ScoreSound;
    public NetworkVariable<int> homeScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> awayScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public PuckBehaviour puck;

    void Awake()
    {
        GameObject homeScoreObject = GameObject.Find("HomeScore");
        if (homeScoreObject != null)
        {
            scoreTextHome = homeScoreObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("HomeScore GameObject not found");
        }
        ScoreSound = GetComponent<AudioSource>();
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
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
        scoreTextHome.text = "Home: " + homeScore.Value.ToString();
        scoreTextAway.text = "Away: " + awayScore.Value.ToString();
        ScoreSound = GetComponent<AudioSource>();

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

        homeScore.OnValueChanged += OnHomeScoreChanged;
        awayScore.OnValueChanged += OnAwayScoreChanged;

        OnHomeScoreChanged(0, homeScore.Value);
        OnAwayScoreChanged(0, awayScore.Value);

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

    [ServerRpc]
    private void UpdateHomeScoreServerRpc()
    {
        homeScore.Value++;
    }

    [ServerRpc]
    private void UpdateAwayScoreServerRpc()
    {
        awayScore.Value++;
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
