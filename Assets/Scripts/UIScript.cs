using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public GameObject PauseScreenUI;
    public GameObject LoseScreenUI;
    public static UIScript instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        } else
        {
            instance = this;
        }
    }

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

    public void PlayerLose()
    {
        LoseScreenUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
