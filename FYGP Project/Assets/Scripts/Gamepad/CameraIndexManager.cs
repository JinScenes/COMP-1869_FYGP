using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class CameraIndexManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    private PlayerInputManager playerInputManager;

    public static Action<int, Transform> OnPlayerSpawn;

    private void Awake()
    {
        VarSetups();
        StartCoroutine(InitializePlayerInputManager());
        PlayerManagerInstanceInitialiser();
    }

    private void VarSetups()
    {
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
