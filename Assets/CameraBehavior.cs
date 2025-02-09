using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private GameObject puck; // The puck object
    void Start()
    {
    }
    void Update()
    {
        //If the puck is not set, find the puck
        if (puck == null)
        {
            puck = GameObject.FindGameObjectWithTag("Puck");
        }
        if (puck != null)
        {
            Vector3 direction = puck.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }
    }
}
