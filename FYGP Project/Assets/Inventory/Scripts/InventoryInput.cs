using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour
{

    public int selection = 0;
    private Inventory playerInventory;

    private GamepadInput controllerInput;
    private PlayerStatsHandler playerStatsHandler;
    [SerializeField] public GunHolder gunHolder;
    private int playerIndex;

    // Values to prevent inputs happening too fast
    private float inputCD = .16f; // Threshold
    private float inputCDTick = 0f;
    
    // Prevents inputs happening too fast
    private bool acceptInput { get { return inputCDTick <= 0 ? true : false; } set { } }
   
    // Clamps selection
    public int Selection
    {
        get { return selection; }
        set
        {
            StartInputTimer();

            // Allows players to press left at 0 and go to 3
            if(value < 0) { value = 3; } else if(value > 3) { value = 0; }

            // was orginaly used to clamp
            selection = value; 
        }
    }

    private void StartInputTimer()
    {
        inputCDTick = inputCD;
    }
    // Start is called before the first frame update
    void Start()
    {
        controllerInput = GetComponent<GamepadInput>();
        playerStatsHandler = GetComponent<PlayerStatsHandler>();   
        playerIndex = controllerInput.playerIndex;
        playerInventory = playerStatsHandler.playerInventory;
    }

    void ShowInvenSelection()
    {
        Transform InvenUI = GameObject.Find($"Inventory{playerIndex}").transform;
        for (int i = 0; i < 4; i++)
        {
            GameObject slotToUpdate = InvenUI.transform.Find($"Slot{i}").gameObject;
            slotToUpdate.GetComponent<Outline>().enabled = false;
        }

        GameObject slotToShow = InvenUI.transform.Find($"Slot{Selection}").gameObject;
        slotToShow.GetComponent<Outline>().enabled = true;

    }
    // Update is called once per frame
    void Update()
    {

        // Handles when player can move right-left (So its not too quick)
        if (inputCDTick > 0)
        {
            inputCDTick -= 1 * Time.deltaTime;
        }

        //Debug.Log(controllerInput.DPadInput);

        if (acceptInput)
        {
            if (controllerInput.DPadInput == Vector2.right)
            {
                Selection++;
            }
            else if (controllerInput.DPadInput == Vector2.left)
            {
                Selection--;
            }

            if (controllerInput.DPadInput == Vector2.up)
            {
                StartInputTimer();
  
                playerInventory.Consume(Selection, gunHolder);

            }
            else if (controllerInput.DPadInput == Vector2.down)
            {
                StartInputTimer();
                playerInventory.DropItem(Selection, transform);
            }
        }
      

        ShowInvenSelection();
    }


}
