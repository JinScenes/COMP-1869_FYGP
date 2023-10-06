using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
public class GamepadInput : MonoBehaviour
{
    //CONTROLLER ID (TO INDICATE WHICH PLAYER IS WHO)
    public int playerIndex { get; private set; }

    private PlayerInput playerInput;
    private Gamepad gamepad;

    private float nextVibrationTime = 0f;
    private float vibrationDelay = 0.1f;
    private bool isVibrating = false;

    /// <summary>
    /// CONTROLLER INPUTS
    /// </summary>

    //STICKS
    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }

    //D-PAD BUTTONS
    public Vector2 DPadInput { get; private set; }

    //A,B,Y,X BUTTONS | TRIANGLE, SQUARE, CIRCLE, X BUTTONS
    public bool ButtonNorth { get; private set; }
    public bool ButtonEast { get; private set; }
    public bool ButtonSouth { get; private set; }
    public bool ButtonWest { get; private set; }

    //OPTIONAL BUTTONS
    public bool SelectButton { get; private set; }
    public bool StartButton { get; private set; }

    //SHOULDER BUTTONS
    public bool LeftShoulder { get; private set; }
    public bool RightShoulder { get; private set; }

    //TRIGGER BUTTONS
    public float LeftTrigger { get; private set; }
    public float RightTrigger { get; private set; }

    //STICK PRESS
    public bool LeftStickPress { get; private set; }
    public bool RightStickPress { get; private set; }

    //TOUCHPAD (PS EXCLUSIVE)
    public bool TouchPadButton { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
        AssignGamepad();
        ControllerInputs();
    }

    private void Update()
    {
        DebuggerFunction();
        AnalogUpdater();
    }

    private void ControllerInputs()
    {
        SticksRotaryMovement();
        D_Pad();
        ShoulderButtons();
        TriggerButtons();
        MainButtons();
        OptionalButtons();
        StickDownButtons();
        TouchPadButtons();
    }

    private void AssignGamepad()
    {
        gamepad = Gamepad.all[playerIndex];
    }

    private void AnalogUpdater()
    {
        //UPDATING ANALOG ACTION
        MovementInput = playerInput.actions["Movement"].ReadValue<Vector2>();
        RotationInput = playerInput.actions["Rotation"].ReadValue<Vector2>();
    }

    #region Controller Inputs

    private void SticksRotaryMovement()
    {
        //MOVEMENT
        playerInput.actions["Movement"].performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Movement"].canceled += ctx => MovementInput = Vector2.zero;

        //ROTATION
        playerInput.actions["Rotation"].performed += ctx => RotationInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Rotation"].canceled += ctx => RotationInput = Vector2.zero;
    }

    private void D_Pad()
    {
        //D-PAD
        playerInput.actions["D-Pad Up"].performed += ctx => DPadInput = Vector2.up;
        playerInput.actions["D-Pad Down"].performed += ctx => DPadInput = Vector2.down;
        playerInput.actions["D-Pad Left"].performed += ctx => DPadInput = Vector2.left;
        playerInput.actions["D-Pad Right"].performed += ctx => DPadInput = Vector2.right;
    }

    private void ShoulderButtons()
    {
        //LEFT SHOULDER
        playerInput.actions["Left Shoulder Button"].performed += ctx => LeftShoulder = true;
        playerInput.actions["Left Shoulder Button"].canceled += ctx => LeftShoulder = false;

        //RIGHT SHOULDER
        playerInput.actions["Right Shoulder Button"].performed += ctx => RightShoulder = true;
        playerInput.actions["Right Shoulder Button"].canceled += ctx => RightShoulder = false;
    }

    private void TriggerButtons()
    {
        //LEFT TRIGGER
        playerInput.actions["Left Trigger"].performed += ctx => LeftTrigger = ctx.ReadValue<float>();
        playerInput.actions["Left Trigger"].canceled += ctx => LeftTrigger = 0;

        //RIGHT TRIGGER
        playerInput.actions["Right Trigger"].performed += ctx => RightTrigger = ctx.ReadValue<float>();
        playerInput.actions["Right Trigger"].canceled += ctx => RightTrigger = 0;
    }

    private void MainButtons()
    {
        //PLAYSTATION | XBOX

        //TRIANGLE | Y
        playerInput.actions["North"].performed += ctx => ButtonNorth = true;
        playerInput.actions["North"].canceled += ctx => ButtonNorth = false;

        //CIRCLE | A
        playerInput.actions["East"].performed += ctx => ButtonEast = true;
        playerInput.actions["East"].canceled += ctx => ButtonEast = false;

        //X | B
        playerInput.actions["South"].performed += ctx => ButtonSouth = true;
        playerInput.actions["South"].canceled += ctx => ButtonSouth = false;

        //SQUARE | X
        playerInput.actions["West"].performed += ctx => ButtonWest = true;
        playerInput.actions["West"].canceled += ctx => ButtonWest = false;
    }

    private void OptionalButtons()
    {
        //SELECT
        playerInput.actions["Select Button"].performed += ctx => SelectButton = true;
        playerInput.actions["Select Button"].canceled += ctx => SelectButton = false;

        //START
        playerInput.actions["Start Button"].performed += ctx => StartButton = true;
        playerInput.actions["Start Button"].canceled += ctx => StartButton = false;
    }

    private void StickDownButtons()
    {
        //LEFT STICK PRRESS
        playerInput.actions["Left Stick Down"].performed += ctx => LeftStickPress = true;
        playerInput.actions["Left Stick Down"].canceled += ctx => LeftStickPress = false;

        //RIGHT STICK PRRESS
        playerInput.actions["Right Stick Down"].performed += ctx => RightStickPress = true;
        playerInput.actions["Right Stick Down"].canceled += ctx => RightStickPress = false;
    }

    private void TouchPadButtons()
    {
        //EXCLUSIVE TO PLAYSTATION
        playerInput.actions["TouchpadButton"].performed += ctx => TouchPadButton = true;
        playerInput.actions["TouchpadButton"].canceled += ctx => TouchPadButton = false;
    }

    #endregion

    #region Vibration

    public void Vibrate(float leftMotor, float rightMotor)
    {
        if (gamepad == null) return;

        gamepad.SetMotorSpeeds(leftMotor, rightMotor);
    }

    public void StopVibration()
    {
        if (gamepad != null) gamepad.SetMotorSpeeds(0, 0);
    }

    #endregion

    public int GetPlayerIndex()
    {
        return playerInput.playerIndex;
    }

    private void DebuggerFunction()
    {
        //ROTATION AND MOVEMENT
        //Debug.Log("Movement Input: " + MovementInput);
        //Debug.Log("Rotation Input: " + RotationInput);

        //D-PAD
        //Debug.Log("D-Pad Input: " + DPadInput);

        //SHOULDER
        //Debug.Log("Left Shoulder: " + LeftShoulder);
        //Debug.Log("Right Shoulder: " + RightShoulder);

        ////TRIGGERS
        //Debug.Log("Left Trigger: " + LeftTrigger);
        //Debug.Log("Right Trigger: " + RightTrigger);

        ////MAIN BUTTONS
        //Debug.Log("Triangle: " + ButtonNorth);
        //Debug.Log("Square: " + ButtonWest);
        //Debug.Log("X: " + ButtonSouth);
        //Debug.Log("Circle: " + ButtonEast);

        ////OPTIONAL BUTTONS
        //Debug.Log("Select: " + SelectButton);
        //Debug.Log("Start: " + StartButton);

        ////STICKDOWN BUTTONS
        //Debug.Log("Left Stick " + LeftStickPress);
        //Debug.Log("Right Stick " + RightStickPress);

        ////TOUCHPAD
        //Debug.Log("Touchpad Button" + TouchPadButton);

        //VIBRATION TEST
        if (RightTrigger > 0 && Time.time > nextVibrationTime)
        {
            Vibrate(0.75f, 0.75f);
            isVibrating = true;
            nextVibrationTime = Time.time + vibrationDelay;
        }
        else if (RightTrigger <= 0 && isVibrating)
        {
            StopVibration();
            isVibrating = false;
        }
    }
}
