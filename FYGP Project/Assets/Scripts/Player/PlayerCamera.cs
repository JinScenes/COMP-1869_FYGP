<<<<<<< HEAD
using UnityEngine;
using System.Collections.Generic;
=======
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
>>>>>>> BranchMerger

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

<<<<<<< HEAD
    private List<Transform> playerTransforms = new List<Transform>();
    private Vector3 lastAveragePosition;
    private Vector3 currentVelocity;

    private void Awake()
    {
        GamepadInputManager.OnPlayerSpawn += AddPlayerTransform;
=======
    [Header("Boundary Alert")]
    [SerializeField] private Image topBoundaryImage;
    [SerializeField] private Image leftBoundaryImage;
    [SerializeField] private Image rightBoundaryImage;
    [SerializeField] private Image bottomBoundaryImage;

    private List<Transform> playerTransforms = new List<Transform>();
    private Vector3 lastAveragePosition;
    private Vector3 currentVelocity;

    private void Awake()
    {
        GamepadInputManager.OnPlayerSpawn += AddPlayerTransform;

        SetImageTransparency(topBoundaryImage, 0);
        SetImageTransparency(bottomBoundaryImage, 0);
        SetImageTransparency(leftBoundaryImage, 0);
        SetImageTransparency(rightBoundaryImage, 0);
    }

    private void Update()
    {
        CheckBoundaryCollision();
>>>>>>> BranchMerger
    }

    private void LateUpdate()
    {
        MoveCameraBasedOnPlayers();
    }

<<<<<<< HEAD
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
=======
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInBorder(bottomBoundaryImage));
        }
    }

    public void AddPlayerTransform(int index, Transform playerTransform)
    {
        if (!playerTransforms.Contains(playerTransform))
        {
            playerTransforms.Add(playerTransform);
>>>>>>> BranchMerger
        }

<<<<<<< HEAD
        Vector3 averagePosition = sumOfPositions / playerTransforms.Count;

        if (topmostPlayer.y > lastAveragePosition.y + 0.1f)
        {
=======
    private void CheckBoundaryCollision()
    {
        foreach (var player in playerTransforms)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(player.position);

            if (viewportPos.y >= 1 - topBoundaryOffset)
            {
                ShowBoundaryLight("Top");
            }
            else if (viewportPos.y <= 0 + bottomBoundaryOffset)
            {
                ShowBoundaryLight("Bottom");
            }

            if (viewportPos.x <= 0 + leftBoundaryOffset)
            {
                ShowBoundaryLight("Left");
            }
            else if (viewportPos.x >= 1 - rightBoundaryOffset)
            {
                ShowBoundaryLight("Right");
            }
        }
    }

    private IEnumerator FadeInBorder(Image boundaryImage)
    {
        float duration = 1f;
        float maxAlpha = .8f;
        float currentLerpTime = 0f;

        while (currentLerpTime < duration)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / duration;
            float alpha = Mathf.Lerp(0f, maxAlpha, perc);
            SetImageTransparency(boundaryImage, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        currentLerpTime = 0f;

        while (currentLerpTime < duration)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / duration;
            float alpha = Mathf.Lerp(maxAlpha, 0f, perc);
            SetImageTransparency(boundaryImage, alpha);
            yield return null;
        }
    }

    private void ShowBoundaryLight(string boundary)
    {
        switch (boundary)
        {
            case "Top":
                StartCoroutine(FlashBoundary(topBoundaryImage));
                break;
            case "Bottom":
                StartCoroutine(FlashBoundary(bottomBoundaryImage));
                break;
            case "Left":
                StartCoroutine(FlashBoundary(leftBoundaryImage));
                break;
            case "Right":
                StartCoroutine(FlashBoundary(rightBoundaryImage));
                break;
        }
    }

    private IEnumerator FlashBoundary(Image boundaryImage)
    {
        float targetAlpha = 0.8f;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, targetAlpha, elapsedTime / duration);
            SetImageTransparency(boundaryImage, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(targetAlpha, 0f, elapsedTime / duration);
            SetImageTransparency(boundaryImage, alpha);
            yield return null;
        }
    }

    private void SetImageTransparency(Image image, float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
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
>>>>>>> BranchMerger
            averagePosition = topmostPlayer;
        }
        else if (bottommostPlayer.y < lastAveragePosition.y - 0.1f)
        {
            averagePosition = bottommostPlayer;
        }

<<<<<<< HEAD
        transform.position = Vector3.SmoothDamp(transform.position, 
=======
        transform.position = Vector3.SmoothDamp(transform.position,
>>>>>>> BranchMerger
            averagePosition + cameraOffset, ref currentVelocity, smoothTime);

        lastAveragePosition = averagePosition;
    }
}