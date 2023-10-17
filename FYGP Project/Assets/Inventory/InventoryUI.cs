using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI 
{
    public int playerIndex;
    public InventoryUI(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    public void ResetUI(Inventory inventoryRef)
    {
        Transform InvenUI = GameObject.Find($"Inventory{playerIndex}").transform;
        for (int i = 0; i < inventoryRef.inventory.Count(); i++)
        {
            // Get objects
            GameObject SlotToUpdate = InvenUI.transform.Find($"Slot{i}").gameObject;
            TextMeshProUGUI StackUI = SlotToUpdate.transform.Find("Stack").gameObject.GetComponent<TextMeshProUGUI>();

            // Update UI
            SlotToUpdate.GetComponent<Image>().sprite = null;
            StackUI.text = "0";

        }
    }
    
    public void UpdateAll(Inventory inventoryRef)
    {
        ResetUI(inventoryRef);
        Transform InvenUI = GameObject.Find($"Inventory{playerIndex}").transform;
        for (int i = 0; i < inventoryRef.inventory.Length; i++)
        {
            // Check if item index exists in inventory
            InventoryItem invenItem = inventoryRef.inventory[i];
            if(invenItem != null)
            {
                // Confirm item
                ItemData itemData = invenItem.itemData;

                // Get objects
                GameObject SlotToUpdate = InvenUI.transform.Find($"Slot{i}").gameObject;
                TextMeshProUGUI StackUI = SlotToUpdate.transform.Find("Stack").gameObject.GetComponent<TextMeshProUGUI>();


                // Update UI
                SlotToUpdate.GetComponent<Image>().sprite = itemData.icon;
                StackUI.text = invenItem.stackSize.ToString();

            }

        }
    }
}
