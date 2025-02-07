using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetTrigger : MonoBehaviour
{
    public TextMeshProUGUI scoreTextHome;
    public TextMeshProUGUI scoreTextAway;
    public AudioSource ScoreSound;
    public int homeScore = 0;
    public int awayScore = 0;
    public PuckBehaviour puck;

    void Start()
    {
        puck = GameObject.FindWithTag("Puck").GetComponent<PuckBehaviour>();
        scoreTextHome.text = "Home: " + homeScore.ToString();
        scoreTextAway.text = "Away: " + awayScore.ToString();
        ScoreSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puck" && this.gameObject.tag == "HomeNet")
        {
            puck.ResetPuck();
            ScoreSound.Play();
            awayScore++;
            scoreTextAway.text = "Away: " + awayScore.ToString();
            Debug.Log(awayScore);
        }
        else if (other.gameObject.tag == "Puck" && this.gameObject.tag == "AwayNet")
        {
            puck.ResetPuck();
            ScoreSound.Play();
            homeScore++;
            scoreTextHome.text = "Home: " + homeScore.ToString();
            Debug.Log(homeScore);
        }
    }
}
