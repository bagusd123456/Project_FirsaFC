using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenScript : MonoBehaviour
{
    public GameObject PauseScreenUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
            if (PauseScreenUI.activeSelf)
            {
                PauseScreenUI.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                PauseScreenUI.SetActive(true);
                Time.timeScale = 0;
            }
                
    }
}