using UnityEngine;
using System.Collections.Generic;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [Range(0, 10), SerializeField] private float lerpSpeed = 1f;
    [Range(0, 1), SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10);

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
        Vector3 averagePosition;
        int movingRight = 0, movingLeft = 0, movingUp = 0, movingDown = 0;

        foreach (var player in playerTransforms)
        {
            sumOfPositions += player.position;

            Vector3 moveDirection = (player.position - lastAveragePosition).normalized;
            if (moveDirection.x > 0.1f) movingRight++;
            if (moveDirection.x < -0.1f) movingLeft++;
            if (moveDirection.y > 0.1f) movingUp++;
            if (moveDirection.y < -0.1f) movingDown++;
        }

        averagePosition = sumOfPositions / playerTransforms.Count;

        if (movingRight > playerTransforms.Count / 2) averagePosition.x += 1;
        if (movingLeft > playerTransforms.Count / 2) averagePosition.x -= 1;
        if (movingUp > playerTransforms.Count / 2) averagePosition.y += 1;
        if (movingDown > playerTransforms.Count / 2) averagePosition.y -= 1;

        transform.position = Vector3.SmoothDamp(transform.position, averagePosition + cameraOffset, ref currentVelocity, smoothTime);

        lastAveragePosition = averagePosition;
    }

}
