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

    private Transform InvenUI;
    private Transform AmmoHUD;

    public InventoryUI(int playerIndex)
    {
        this.playerIndex = playerIndex;

        InvenUI = GameObject.Find($"Inventory{playerIndex}").transform;
        AmmoHUD = InvenUI.transform.Find("AmmoHUD");
    }

    public void ResetUI(Inventory inventoryRef)
    {
        for (int i = 0; i < inventoryRef.inventory.Count(); i++)
        {
            // Get slot UI
            GameObject SlotToUpdate = InvenUI.transform.Find($"Slot{i}").gameObject;

            // Get Refs
            TextMeshProUGUI StackUI = SlotToUpdate.transform.Find("Stack").gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI DisplayName = SlotToUpdate.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>(); ;

            GameObject SlotImage = SlotToUpdate.transform.Find("Icon").gameObject;
            GameObject Consumable = SlotToUpdate.transform.Find("Consumable").gameObject;

            // Update UI
            SlotImage.GetComponent<Image>().sprite = null;
            Consumable.GetComponent<Image>().enabled = false;
            StackUI.text = "";
            DisplayName.text = "";

        }
    }
    
    public void UpdateAll(Inventory inventoryRef)
    {
        ResetUI(inventoryRef);
        for (int i = 0; i < inventoryRef.inventory.Length; i++)
        {
            // Check if item index exists in inventory
            InventoryItem invenItem = inventoryRef.inventory[i];
            if(invenItem != null)
            {
                // Confirm item
                ItemData itemData = invenItem.itemData;

                // Get slot UI
                GameObject SlotToUpdate = InvenUI.transform.Find($"Slot{i}").gameObject;

                // Get Refs
                TextMeshProUGUI StackUI = SlotToUpdate.transform.Find("Stack").gameObject.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI DisplayName = SlotToUpdate.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>(); ;

                GameObject SlotImage = SlotToUpdate.transform.Find("Icon").gameObject;
                GameObject Consumable = SlotToUpdate.transform.Find("Consumable").gameObject;

                // Update UI
                SlotImage.GetComponent<Image>().sprite = itemData.icon;
                StackUI.text = invenItem.stackSize.ToString();
                DisplayName.text = itemData.name;

                if (itemData.consumable){
                    Consumable.GetComponent<Image>().enabled = true;
                }

            }

        }
    }

    public void UpdateAllAmmo(PlayerAmmo playerAmmo)
    {
        
        TextMeshProUGUI LargeAmmoUI = AmmoHUD.Find("Large Ammo").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI MediumAmmoUI = AmmoHUD.Find("Medium Ammo").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI SmallAmmoUI = AmmoHUD.Find("Small Ammo").GetComponent<TextMeshProUGUI>();

        LargeAmmoUI.text = playerAmmo.largeAmmo.ammount.ToString();
        MediumAmmoUI.text = playerAmmo.mediumAmmo.ammount.ToString();
        SmallAmmoUI.text = playerAmmo.smallAmmo.ammount.ToString();
    }

    public void UpdateSmallAmmo(int ammo)
    {
        TextMeshProUGUI SmallAmmoUI = AmmoHUD.Find("Small Ammo").GetComponent<TextMeshProUGUI>();
        SmallAmmoUI.text = ammo.ToString();
    }

    public void UpdateMediumAmmo(int ammo)
    {
        TextMeshProUGUI MediumAmmoUI = AmmoHUD.Find("Medium Ammo").GetComponent<TextMeshProUGUI>();
        MediumAmmoUI.text = ammo.ToString();
    }

    public void UpdateLargeAmmo(int ammo)
    {
        TextMeshProUGUI LargeAmmoUI = AmmoHUD.Find("Large Ammo").GetComponent<TextMeshProUGUI>();
        LargeAmmoUI.text = ammo.ToString();
    }


}
