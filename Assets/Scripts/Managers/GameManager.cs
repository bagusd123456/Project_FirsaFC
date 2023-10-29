using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnGameStartAction;

    public List<Transform> spawnPoint = new List<Transform>();
    public CinemachineTargetGroup cinemachineTargetGroup;



    public static GameManager Instance { get; private set; }
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
    // Start is called before the first frame update
    void Start()
    {
        OnGameStartAction?.Invoke();

        AssignPlayerToSpawnPosition();
    }

    public void AssignPlayerToSpawnPosition()
    {
        var playerList = PlayerManager.Instance.spawnedPlayerList;
        for (int i = 0; i < playerList.Count; i++)
        {
            PlayerManager.Instance.spawnedPlayerList[i].transform.position = spawnPoint[i].position;
            cinemachineTargetGroup.AddMember(playerList[i].transform, 1, 1);
        }
    }

    public void CheckLoseCondition()
    {
        var playerList = PlayerManager.Instance.spawnedPlayerList;
        int playerCount = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].GetComponent<BasePlayerController>().isDead)
            {
                playerCount++;
                cinemachineTargetGroup.RemoveMember(playerList[i].transform);
            }
        }

        if (playerCount >= playerList.Count)
        {
            UIScript.instance.PlayerLose();
        }
    }
}
