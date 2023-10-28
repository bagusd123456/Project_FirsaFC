using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLimitBehaviour : MonoBehaviour
{
    //public Transform target;
    public LaserObjectBehaviour[] laser;
    public float speed;
    public float timeDelay;

    bool playerIsInRange = false;
    bool playerHasPassed;

    // Update is called once per frame
    private void FixedUpdate()
    {
        StartCoroutine(StartObstacle());
    }

    // Add speed to laser
    IEnumerator StartObstacle()
    {
        if (playerIsInRange)
        {
            for (int i = 0; i < laser.Length; i++)
            {
                laser[i].timeToStop = false;

                yield return new WaitForSeconds(timeDelay);
            }
            playerIsInRange = false;
        }
        
    }

    // Start laser when player stepped on the collider trigger
    // Stops laser when laser is on end track
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerHasPassed)
        {
            playerIsInRange = true;
            playerHasPassed = true;
        }
    }
}
