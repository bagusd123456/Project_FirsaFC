using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Multiplayer_PanelUI : MonoBehaviour
{
    public List<PlayerInput> playerInputs = new List<PlayerInput>();
    public List<TMP_Text> playerTexts = new List<TMP_Text>();
    public static Multiplayer_PanelUI Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        //UpdateText();
    }

    public void UpdateText()
    {
        if(PlayerManager.Instance.playerDevices.Count < 0) return;

        var deviceList = PlayerManager.Instance.playerDevices;
        for (int i = 0; i < deviceList.Count; i++)
        {
            playerTexts[i].text = $"Player {i+1} \n{deviceList[i]}";
        }
    }

    public void ToggleShowPanel(int index)
    {
        var GO = playerTexts[index].gameObject;
        GO.SetActive(!GO.activeSelf);
    }

    public void StartGameMultiplayer()
    {
        PlayerManager.Instance.playerCount = PlayerManager.Instance.playerDevices.Count;
        SceneManager.LoadScene("Level 1 Multiplayer");
    }
}
