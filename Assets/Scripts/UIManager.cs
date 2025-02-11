using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] UIs;//The UI objects
    [Header("Win Stats")]
    public TextMeshProUGUI winText;
    public TextMeshProUGUI goalText;
    void Start()
    {
       SetAllFalse();
        LoadUI("MainMenuUI");
    }
    void SetAllFalse()
    {
        //Loop through all the UI objects
        foreach(GameObject UI in UIs)
        {
            //Disable the UI object
            UI.SetActive(false);
        }
    }
    /// <summary>
    /// Load the UI with the given name
    /// </summary>
    /// <param name="name">The name of the UI to load</param>
    public void LoadUI(string name)
    {
        //Loop through all the UI objects
        foreach(GameObject UI in UIs)
        {
            //If the UI object has the same name as the given name
            if(UI.name == name)
            {
                //Enable the UI object
                UI.SetActive(true);
            }
            else
            {
                //Disable the UI object
                UI.SetActive(false);
            }
        }
    }
}
