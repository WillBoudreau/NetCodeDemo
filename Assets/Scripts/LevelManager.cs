using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LevelManager : NetworkBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NetworkUI networkUI;
    public string levelToLoad;

    void Start()
    {
        networkUI = FindObjectOfType<NetworkUI>();
    }
    /// <summary>
    /// Load the level with the given name asynchronously
    /// </summary>
    public void LoadLevel(string lvl)
    {
        StartCoroutine(LoadLevelAsync(lvl));
    }

    private IEnumerator LoadLevelAsync(string lvl)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(lvl);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (lvl == "Game")
        {
            gameManager.DisableCamera();
            levelToLoad = lvl;
            if(!IsClient)
            {
                networkUI.StartHost();
            }
            else
            {
                networkUI.StartClient();
            }
        }
    }
}
