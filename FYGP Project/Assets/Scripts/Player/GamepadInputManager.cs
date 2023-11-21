using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class GamepadInputManager : MonoBehaviour
{
    public GameObject playerCamera;
    public Transform[] spawnPoints;

    private TransformManager transformManager;
    private PlayerInputManager playerInputManager;

    private int currentPlayerCount = 0;

    public static Action<int, Transform> OnPlayerSpawn;

    private void Awake()
    {
        VarSetups();
        StartCoroutine(InitializePlayerInputManager());
        PlayerManagerInstanceInitialiser();
    }

    private void VarSetups()
    {
        transformManager = GetComponent<TransformManager>();
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnDestroy()
    {
        if (PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
        }
    }

    private void PlayerManagerInstanceInitialiser()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        }
        else
        {
            Debug.LogWarning("PlayerInputManager instance is not set");
        }
    }

    private IEnumerator InitializePlayerInputManager()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (currentPlayerCount < spawnPoints.Length)
        {
            playerInput.transform.position = spawnPoints[currentPlayerCount].position;
            currentPlayerCount++;
        }
        else
        {
            Debug.LogWarning("All spawn points are occupied!");

        }

        if (transformManager)
        {
            transformManager.RegisterPlayer(playerInput.gameObject);
        }
        else
        {
            Debug.LogWarning("TransformManager not set in GamepadInputManager");

        }

        OnPlayerSpawn?.Invoke(playerInput.playerIndex, playerInput.transform);
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }
}
