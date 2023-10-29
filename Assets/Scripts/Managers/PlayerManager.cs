using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cinemachine.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public InputAction playerInputAction;

    public List<PlayerInput> players = new List<PlayerInput>();
    public List<InputDevice> playerDevices = new List<InputDevice>();

    [SerializeField] private List<Transform> startingPoints;

    [SerializeField] private List<LayerMask> playerLayers;

    private PlayerInputManager playerInputManager;
    public GameObject playerPrefab;

    public List<TMP_Text> playerNames = new List<TMP_Text>();

    public int playerCount;
    public static PlayerManager Instance { get; private set; }

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

        playerInputManager = FindObjectOfType<PlayerInputManager>();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //Multiplayer_PanelUI.Instance.playerInputs = players;
        //Multiplayer_PanelUI.Instance.UpdateText();
    }

    //private void InputOnChange(object arg1, InputActionChange arg2)
    //{
    //    if (arg2 == InputActionChange.ActionPerformed)
    //    {
    //        InputAction receivedInputAction = (InputAction) arg1;
    //        InputDevice lastDevice = receivedInputAction.activeControl.device;

    //        bool isKeyboard = lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse");
    //        bool alreadyJoin = false;

    //        for (int i = 0; i < playerDevices.Count; i++)
    //        {
    //            if (playerDevices[i] == lastDevice)
    //            {
    //                alreadyJoin = true;
    //                Debug.Log($"Device {lastDevice} Already Registered");
    //                break;
    //            }
    //        }

    //        if (!alreadyJoin)
    //        {
    //            playerDevices.Add(lastDevice);
    //            Debug.Log($"Device {lastDevice} not Registered");
    //        }

    //        if (isKeyboard)
    //        {
    //            //var playerInput = playerInputManager.JoinPlayer(players.Count, -1, "Keyboard", lastDevice);
    //            //players.Add(playerInput);
    //        }
    //        else
    //        {
    //            //var playerInput = playerInputManager.JoinPlayer(players.Count, -1, "Joystick", lastDevice);
    //            //players.Add(playerInput);
    //        }
    //        Debug.Log($"Input is Keyboard = {isKeyboard}");
    //    }
    //}

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += PlayerInputManager_onPlayerJoined;
        playerInputManager.onPlayerLeft += PlayerInputManager_onPlayerLeft;
        GameManager.OnGameStartAction += SpawnPlayerList;

        SceneManager.sceneLoaded += SpawnPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= PlayerInputManager_onPlayerJoined;
        playerInputManager.onPlayerLeft -= PlayerInputManager_onPlayerLeft;

        GameManager.OnGameStartAction -= SpawnPlayerList;
    }

    private void PlayerInputManager_onPlayerJoined(PlayerInput playerInput)
    {
        //JoinPlayer(playerInput);
        playerCount++;
        playerDevices.Add(playerInput.devices[0].device);
        Multiplayer_PanelUI.Instance.UpdateText();
        Debug.Log($"Player Joined: {playerInput.devices}");
    }

    private void PlayerInputManager_onPlayerLeft(PlayerInput playerInput)
    {
        //playerDevices.Remove(playerInput.devices[0].device);

        Multiplayer_PanelUI.Instance.UpdateText();
        Debug.Log($"Player Disconnected: {playerInput.devices}");
    }

    public void JoinPlayer(PlayerInput playerInput)
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerNames[i].text = players[i].devices[0].name;
        }
    }

    public void LeavePlayer(PlayerInput playerInput)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == playerInput)
            {
                players.RemoveAt(i);
                playerNames[i].text = $"Player {i + 1} Disconnected: ";
            }
        }
    }

    [ContextMenu("Spawn Player")]
    public void SpawnPlayerList()
    {
        Debug.Log("SpawnPlayer Called");
        //playerDevices = InputSystem.devices.ToList();

        //for (int i = 0; i < playerDevices.Count; i++)
        //{
        //    //SpawnPlayer(players[i]);

        //    var playerInput = playerInputManager.JoinPlayer(i, -1, pairWithDevice: playerDevices[i]);
        //    Debug.LogWarning($"Added controller: {playerInput.devices[0].displayName}");
        //}
    }

    public void SpawnPlayer(Scene arg0, LoadSceneMode arg1)
    {
        var spawnedPlayerList = new List<PlayerInput>();
        for (int i = spawnedPlayerList.Count; i < playerCount; i++)
        {
            //var GO = Instantiate(playerPrefab);

            if (i > playerDevices.Count) return;
            var lastDevice = playerDevices[i];
            PlayerInput player;
            bool isKeyboard = lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse");

            if (isKeyboard)
            {
                player = PlayerInput.Instantiate(playerPrefab, i, controlScheme: "Keyboard", -1,
                    pairWithDevice: playerDevices[i]);
            }
            else
            {
                player = PlayerInput.Instantiate(playerPrefab, i, controlScheme: "Joystick", -1,
                    pairWithDevice: playerDevices[i]);
            }
            
            spawnedPlayerList.Add(player);
        }
        Debug.Log($"Spawning Player");
    }
}
