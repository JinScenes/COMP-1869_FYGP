using UnityEngine.InputSystem;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GamepadInput))]
public class PlayerController : MonoBehaviour
{
    [Range(0, 20),
    SerializeField] private float speed;

    public GameObject gunRef;
    private GamepadInput controllerInput;

    private void Awake()
    {
        VariableComponents();
        AnnouncePlayerSpawn();
    }

    private void Start()
    {
        AssignPlayerCamera();
        StartCoroutine(ActivatePlayerInput());
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        CheckShooting();
    }

    private void VariableComponents()
    {
        controllerInput = GetComponent<GamepadInput>();
        GetComponent<PlayerInput>().camera = FindPlayerCamera();
    }

    private void CheckShooting()
    {
        if (controllerInput.RightTrigger > 0.5f)
        {
            ShootingFunction();
        }
    }

    private Camera FindPlayerCamera()
    {
        var playerCam = FindObjectsOfType<PlayerCamera>().FirstOrDefault(cam => cam.playerIndex 
        == controllerInput.GetPlayerIndex());

        return playerCam ? playerCam.GetComponent<Camera>() : null;
    }

    private IEnumerator ActivatePlayerInput()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerInput>().enabled = true;
    }

    private void AssignPlayerCamera()
    {
        var playerCam = FindObjectsOfType<PlayerCamera>().FirstOrDefault(cam => cam.playerIndex 
        == controllerInput.GetPlayerIndex());

        if (playerCam != null && playerCam.GetComponent<Camera>())
        {
            GetComponent<PlayerInput>().camera = playerCam.GetComponent<Camera>();
        }
    }

    private void ShootingFunction()
    {
        gunRef.GetComponent<GunBase>().Fire();
    }
    private void AnnouncePlayerSpawn()
    {
        PlayerCamera.OnPlayerSpawn?.Invoke(GetComponent<PlayerInput>().playerIndex, transform);
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(controllerInput.MovementInput.x, 0, 
            controllerInput.MovementInput.y);

        Vector3 moveOffset = moveDirection * speed * Time.deltaTime;
        transform.position += moveOffset;
    }

    private void HandleRotation()
    {
        if (controllerInput.RotationInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(controllerInput.RotationInput.x, 
                controllerInput.RotationInput.y) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
