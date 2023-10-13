using UnityEngine.InputSystem;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Variables")]
    public int playerIndex = 0;

    public Transform playerTransform;

    [Header("Camera Stats")]
    [Range(0, 10)] public float lerpSpeed;
    [Range(0, 1)] public float smoothZoomTime;
    [Range(0, 10)] public float targetOrthographicSize;

    private float currentVelocityZoom = 0f;
    private float joinButtonCooldown = 0.5f;
    private float lastJoinButtonPressTime = 0f;

    public Vector3 cameraOffset;
    public Vector3 rotationAngles;

    private Camera cam;

    public static Action<int, Transform> OnPlayerSpawn;

    private void Start()
    {
        StartCoroutine(InitializePlayerInputManager());
        VariableComponents();
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            FollowPlayer();
            Zooming();

            Debug.Log("Camera " + playerIndex + " received spawn announcement for player " + playerIndex);

        }
    }

    private void OnDestroy()
    {
        if (PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
        }
    }

    private void VariableComponents()
    {
        cam = GetComponent<Camera>();
        PlayerManagerInstanceInitialiser();
        var players = GameObject.FindGameObjectsWithTag("Player");
        var matchingPlayer = players.FirstOrDefault(p => 
        p.GetComponent<PlayerInput>()?.playerIndex == playerIndex);

        if (matchingPlayer != null)
        {
            playerTransform = matchingPlayer.transform;
            matchingPlayer.GetComponent<PlayerInput>().camera = cam;
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
        if (playerInput.playerIndex == playerIndex)
        {
            playerInput.camera = cam;

            var players = GameObject.FindGameObjectsWithTag("Player");
            var matchingPlayer = players.FirstOrDefault(p =>
            p.GetComponent<PlayerInput>().playerIndex == playerIndex);

            if (matchingPlayer != null)
            {
                playerTransform = matchingPlayer.transform;
            }
        }
    }

    private void HandlePlayerSpawn(int spawnedPlayerIndex, Transform spawnedPlayerTransform)
    {
        if (spawnedPlayerIndex == playerIndex)
        {
            playerTransform = spawnedPlayerTransform;
            spawnedPlayerTransform.GetComponent<PlayerInput>().camera = cam;
        }
    }

    private void FollowPlayer()
    {
        Vector3 desiredPos = playerTransform.position + cameraOffset;
        //desiredPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, desiredPos, lerpSpeed * Time.deltaTime);
    }

    private void Zooming()
    {
        float newOrthoSize = Mathf.SmoothDamp(Camera.main.orthographicSize,
            targetOrthographicSize, ref currentVelocityZoom, smoothZoomTime);
        cam.orthographicSize = newOrthoSize;
    }

    private void OnJoinButton()
    {

    }

    private void OnEnable()
    {
        OnPlayerSpawn += HandlePlayerSpawn;
    }

    private void OnDisable()
    {
        OnPlayerSpawn -= HandlePlayerSpawn;
    }
}
