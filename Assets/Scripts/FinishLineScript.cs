using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineScript : MonoBehaviour
{
    public GameObject finishUI;
    // Start is called before the first frame update
    

    // Detects Player Object when crossing the finish line/collider trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            finishUI.SetActive(true);
        }
    }
}
