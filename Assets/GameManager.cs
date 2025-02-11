using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private Camera mainCamera; // The main camera
    [SerializeField] private UIManager uiManager; // The UI Manager
    [SerializeField] private PuckBehaviour puckBehaviour;// Puck behavior
    public NetworkVariable<int> homeScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> awayScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public int homeScoreMaxValue;
    public int awayScoreMaxValue;
    /// <summary>
    /// The instance of the GameManager
    /// </summary>
    public static GameManager instance;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    void Update()
    {
        WinGame();
    }

    public void WinGame()
    {
        if (homeScore.Value >= homeScoreMaxValue)
        {
            homeScore.Value = homeScoreMaxValue;
            uiManager.LoadUI("WinUI");
            uiManager.winText.text = "Home Team Wins!";
            uiManager.goalText.text = "Final Score: " + homeScore.Value + " - " + awayScore.Value;
        }
        else if (awayScore.Value >= awayScoreMaxValue)
        {
            awayScore.Value = awayScoreMaxValue;
            uiManager.LoadUI("WinUI");
            uiManager.winText.text = "Away Team Wins!";
            uiManager.goalText.text = "Final Score: " + homeScore.Value + " - " + awayScore.Value;
        }
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    /// <summary>
    ///Disable the camera
    /// </summary>
    public void DisableCamera()
    {
        //Disable the camera
        mainCamera.enabled = false;
    }
    /// <summary>
    /// Enable the camera
    /// </summary>
    public void EnableCamera()
    {
        //Enable the camera
        mainCamera.enabled = true;
    }
    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        //Pause the game
        Time.timeScale = 0;
    }
    /// <summary>
    /// Unpause the game
    /// </summary>
    public void ResumeGame()
    {
        //Unpause the game
        Time.timeScale = 1;
    }
}
