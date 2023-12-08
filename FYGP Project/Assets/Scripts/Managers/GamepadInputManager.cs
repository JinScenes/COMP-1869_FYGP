using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Material[] playerMaterials;
    [SerializeField] private Color[] playerColours;

    private TransformManager transformManager;
    private PlayerInputManager playerInputManager;

    public static int currentPlayerCount { get; private set; } = 0;

    public static Action<int, Transform> OnPlayerSpawn;

    private void Awake()
    {
        VarSetups();
        StartCoroutine(InitializePlayerInputManager());
        PlayerManagerInstanceInitialiser();
    }

    private void Update()
    {
        Debug.Log(currentPlayerCount);
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

            if (currentPlayerCount < playerMaterials.Length)
            {
                SkinnedMeshRenderer skinnedMeshRenderer = playerInput.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer != null)
                {
                    skinnedMeshRenderer.material = playerMaterials[currentPlayerCount];
                }
            }

            PlayerColourChanger colourChanger = playerInput.gameObject.GetComponentInChildren<PlayerColourChanger>();
            if (colourChanger != null)
            {
                colourChanger.ChangeColour(playerColours[currentPlayerCount]);

            }

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
