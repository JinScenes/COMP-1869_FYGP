using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [Tooltip("How fast the player is able to move")]
    [Range(0, 20)] public float speed;
    [Range(0, 1)] public float movementSmoothness;

    private Vector3 currentVel;
    private Vector2 leftStickInput;
    private Vector2 rightStickInput;

    private Rigidbody rb;

    private PlayerControls input;
    private InputAction movementAction;
    private InputAction rotationAction;

    private void Awake()
    {
        input = new PlayerControls();
        rb = GetComponent<Rigidbody>();

        //INPUT MOVEMENT CALLBACK
        input.Player.Movement.performed += ctx => leftStickInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => leftStickInput = Vector2.zero;

        //INPUT ROTATION CALLBACK
        input.Player.Rotation.performed += ctx => rightStickInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        PlayerMovement();
        PlayerRotation();
        UpdateController();
    }

    private void UpdateController()
    {
        leftStickInput = input.Player.Movement.ReadValue<Vector2>();
        rightStickInput = input.Player.Rotation.ReadValue<Vector2>();
    }

    private void PlayerMovement()
    {
        Vector3 targetVelocity = new Vector3(leftStickInput.x, 0, leftStickInput.y) * speed;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVel, movementSmoothness);
    }

    private void PlayerRotation()
    {
        if(rightStickInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rightStickInput.x, rightStickInput.y) * Mathf.Rad2Deg;
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
