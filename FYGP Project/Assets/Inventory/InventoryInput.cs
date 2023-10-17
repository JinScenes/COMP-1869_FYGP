using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour
{
    public int selection = 0;
    private float selectionCD = .16f;

    private float selectionCDTick = 0f;
    public int Selection
    {
        get { return selection; }
        set
        {
            if (selectionCDTick <= 0)
            {
                selectionCDTick = selectionCD;
                selection = Mathf.Clamp(value, 0, 3); 
            } 
        }
    }
    private GamepadInput controllerInput;


    // Start is called before the first frame update
    void Start()
    {
        controllerInput = GetComponent<GamepadInput>();
    }

    void ShowInvenSelection()
    {
        Transform InvenUI = GameObject.Find($"Inventory{0}").transform;
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
        if (selectionCDTick > 0)
        {
            selectionCDTick -= 1 * Time.deltaTime;
        }
        //Debug.Log(controllerInput.DPadInput);
        if (controllerInput.DPadInput == Vector2.right)
        {
            Selection ++;
        } else if (controllerInput.DPadInput == Vector2.left)
        {
            Selection--;
        }

        ShowInvenSelection();
     

        //print(Selection);
    }


}
