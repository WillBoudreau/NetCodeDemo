using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // The main camera
    /// <summary>
    /// The instance of the GameManager
    /// </summary>
    public static GameManager instance;

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
}
