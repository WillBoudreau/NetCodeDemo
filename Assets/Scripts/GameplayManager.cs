using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [Header("Gameplay Controls")]
    [SerializeField] private float timer; // The time to wait
    [SerializeField] private float minTime; // The time to wait
    [SerializeField] private float maxTime; // The time to wait
    [SerializeField] private bool isOver; // The win state
    [SerializeField] private NetTrigger netTrigger; // The net behavior
    /// <summary>
    /// Gameplay timer
    /// </summary>
   public void Timer()
   {
        timer -= Time.deltaTime;
        if (timer <= minTime)
        {
           isOver = true;
        }
   }
   /// <summary>
   ///  Restart the game
   /// </summary>
   public void RestartGame()
   {
       isOver = false;
       timer = maxTime;
   }
}
