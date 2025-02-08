using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NetworkUI networkUI;

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
            networkUI.StartHost();
        }
    }
}
