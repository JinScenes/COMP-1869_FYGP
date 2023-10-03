using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [Tooltip("How fast the player is able to move")]
    [Range(0, 20)] public float speed;
    [Range(0, 1)] public float movementSmoothness;

    private Vector3 currentVel;
    private Vector2 leftJoystickInput;
    private Vector2 rightJoystickInput;

    private Rigidbody rb;

    private PlayerControls input;
    private InputAction movementAction;
    private InputAction rotationAction;

    private void Awake()
    {
        input = new PlayerControls();
        rb = GetComponent<Rigidbody>();

        //INPUT MOVEMENT CALLBACK
        input.Player.Movement.performed += ctx => leftJoystickInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => leftJoystickInput = Vector2.zero;

        //INPUT ROTATION CALLBACK
        input.Player.Rotation.performed += ctx => rightJoystickInput = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void Update()
    {
        PlayerRotation();
        UpdateGamepad();
    }

    private void UpdateGamepad()
    {
        leftJoystickInput = input.Player.Movement.ReadValue<Vector2>();
        rightJoystickInput = input.Player.Rotation.ReadValue<Vector2>();
    }

    private void PlayerMovement()
    {
        Vector3 targetVelocity = new Vector3(leftJoystickInput.x, 0, leftJoystickInput.y) * speed;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVel, movementSmoothness);
    }

    private void PlayerRotation()
    {
        if(rightJoystickInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rightJoystickInput.x, rightJoystickInput.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    private void OnEnable()
    {
        movementAction = input.Player.Movement;
        movementAction.Enable();

        rotationAction = input.Player.Rotation;
        rotationAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.Disable();
        rotationAction.Disable();
    }
}
