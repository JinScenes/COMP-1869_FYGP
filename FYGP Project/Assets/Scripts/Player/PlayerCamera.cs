using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

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
    }

    private void LateUpdate()
    {
        MoveCameraBasedOnPlayers();
    }

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
        }
    }

    private void CheckBoundaryCollision()
    {
        foreach (var player in playerTransforms)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(player.position);
            //Debug.Log("Viewport Position: " + viewportPos);

            if (viewportPos.y >= 1 - topBoundaryOffset)
            {
                //Debug.Log("Top boundary collision detected.");
                ShowBoundaryLight("Top");
            }
            else if (viewportPos.y <= 0 + bottomBoundaryOffset)
            {
                //Debug.Log("Bottom boundary collision detected.");
                ShowBoundaryLight("Bottom");
            }

            if (viewportPos.x <= 0 + leftBoundaryOffset)
            {
                //Debug.Log("Left boundary collision detected.");
                ShowBoundaryLight("Left");
            }
            else if (viewportPos.x >= 1 - rightBoundaryOffset)
            {
                //Debug.Log("Right boundary collision detected.");
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
        float targetAlpha = 0.8f; // The transparency level when the boundary is fully visible
        float duration = 0.5f; // How long it takes to fade in
        float elapsedTime = 0;

        // Fade in
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, targetAlpha, elapsedTime / duration);
            SetImageTransparency(boundaryImage, alpha);
            yield return null;
        }

        // Keep the image visible for a short time
        yield return new WaitForSeconds(0.5f);

        // Fade out
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