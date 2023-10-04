using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class GamepadInput : MonoBehaviour
{
    public Vector2 CurrentMovementInput { get; private set; }
    public Vector2 CurrentRotationInput { get; private set; }

    private PlayerInput playerInput;

    public int playerIndex { get; private set; }

    public float ShootValue { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;

        //MOVEMENT ACTION
        playerInput.actions["Movement"].performed += ctx => CurrentMovementInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Movement"].canceled += ctx => CurrentMovementInput = Vector2.zero;

        //SHOOTING ACTION
        playerInput.actions["Rotation"].performed += ctx => CurrentRotationInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Rotation"].canceled += ctx => CurrentRotationInput = Vector2.zero;

        //SHOOTING ACTION
        playerInput.actions["TriggerButtons"].performed += ctx => ShootValue = ctx.ReadValue<float>();
        playerInput.actions["TriggerButtons"].canceled += ctx => ShootValue = 0f;
    }

    private void Update()
    {
        DebuggerFunction();
        InputUpdater();
    }

    private void InputUpdater()
    {
        //UPDATING ACTIONS
        CurrentMovementInput = playerInput.actions["Movement"].ReadValue<Vector2>();
        CurrentRotationInput = playerInput.actions["Rotation"].ReadValue<Vector2>();
    }

    public int GetPlayerIndex()
    {
        return playerInput.playerIndex;
    }

    private void DebuggerFunction()
    {
        //Debug.Log("Movement Input: " + CurrentMovementInput);
    }
}
