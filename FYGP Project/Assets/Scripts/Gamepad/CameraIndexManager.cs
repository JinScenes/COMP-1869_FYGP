using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class CameraIndexManager : MonoBehaviour
{
    public GameObject[] cameraGameObjects;

    private IEnumerator Start()
    {
        yield return null;
        PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;

        for (int i = 1; i < cameraGameObjects.Length; i++)
        {
            cameraGameObjects[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;
    }

    private void OnDisable()
    {
        PlayerInputManager.instance.onPlayerJoined -= HandlePlayerJoined;
    }


    private void HandlePlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined with index: " + playerInput.playerIndex);

        int playerIndex = playerInput.playerIndex;
        if (playerIndex < cameraGameObjects.Length)
        {
            cameraGameObjects[playerIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Camera for player index " + playerIndex + " not set up!");
        }
    }
}
