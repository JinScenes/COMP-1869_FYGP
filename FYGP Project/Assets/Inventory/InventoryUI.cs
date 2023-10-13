using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI 
{
    public int playerIndex;
    public InventoryUI(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }
    
    public void UpdateAll(Inventory inventoryRef)
    {
        Debug.Log("UI CAlled");
        for (int i = 0; i < inventoryRef.inventory.Count; i++)
        {
            Transform InvenUI = GameObject.Find($"Inventory{0}").transform;
            GameObject SlotToUpdate = InvenUI.transform.Find($"Slot{i}").gameObject;
            //GameObject newThing = SlotToUpdate.transform.Find("Stack").gameObject;
            TextMeshProUGUI StackUI = SlotToUpdate.transform.Find("Stack").gameObject.GetComponent<TextMeshProUGUI>();

            InventoryItem invenItem = inventoryRef.inventory[i];
            ItemData itemData = invenItem.itemData;

            Debug.Log($"Found {itemData.displayName} with stack {invenItem.stackSize}");

            SlotToUpdate.GetComponent<Image>().sprite = itemData.icon;
            StackUI.text = invenItem.stackSize.ToString();
            //SlotToUpdate.
            //inventoryRef.inventory[i]
        }
    }
}
