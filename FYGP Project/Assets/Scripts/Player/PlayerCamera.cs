using UnityEngine;
using System.Collections.Generic;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Boundary Settings")]
    [Range(0, 0.5f)] public float topBoundaryOffset = 0.05f;
    [Range(0, 0.5f)] public float bottomBoundaryOffset = 0.05f;
    [Range(0, 0.5f)] public float leftBoundaryOffset = 0.05f;
    [Range(0, 0.5f)] public float rightBoundaryOffset = 0.05f;

    [Header("Camera Stats")]
    [Range(0, 10), SerializeField] private float lerpSpeed;
    [Range(0, 1), SerializeField] private float smoothTime;
    [SerializeField] private Vector3 cameraOffset;

    private List<Transform> playerTransforms = new List<Transform>();
    private Vector3 lastAveragePosition;
    private Vector3 currentVelocity;

    private void Awake()
    {
        GamepadInputManager.OnPlayerSpawn += AddPlayerTransform;
    }

    private void LateUpdate()
    {
        MoveCameraBasedOnPlayers();
    }

    public void AddPlayerTransform(int index, Transform playerTransform)
    {
        if (!playerTransforms.Contains(playerTransform))
        {
            playerTransforms.Add(playerTransform);
        }
    }

    private void MoveCameraBasedOnPlayers()
    {
        if (playerTransforms.Count == 0) return;

        Vector3 sumOfPositions = Vector3.zero;
        Vector3 topmostPlayer = new Vector3(0, float.MinValue, 0);
        Vector3 bottommostPlayer = new Vector3(0, float.MaxValue, 0);

        foreach (var player in playerTransforms)
        {
            sumOfPositions += player.position;
            if (player.position.y > topmostPlayer.y) topmostPlayer 
                    = player.position;

            if (player.position.y < bottommostPlayer.y) bottommostPlayer 
                    = player.position;
        }

        Vector3 averagePosition = sumOfPositions / playerTransforms.Count;

        if (topmostPlayer.y > lastAveragePosition.y + 0.1f)
        {
            averagePosition = topmostPlayer;
        }
        else if (bottommostPlayer.y < lastAveragePosition.y - 0.1f)
        {
            averagePosition = bottommostPlayer;
        }

        transform.position = Vector3.SmoothDamp(transform.position, 
            averagePosition + cameraOffset, ref currentVelocity, smoothTime);

        lastAveragePosition = averagePosition;
    }
}
