using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateHealthUI : MonoBehaviour
{
    int playerIndex;
    GameObject inventory;
    TextMeshProUGUI healthText;
    PlayerController playerController;
    // Start is called before the first frame update
    void Awake()
    {
        playerIndex = GetComponent<GamepadInput>().playerIndex;
        playerController = GetComponent<PlayerController>();
        inventory = GameObject.Find("Inventory" + playerIndex);
        healthText = inventory.transform.Find("lblHP").transform.Find("txtHP").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        if (healthText != null)
        {
            healthText.text = Mathf.FloorToInt(Mathf.Clamp(playerController.currentHealth, 0, 1000)) + "/" + playerController.maxHealth;
        }
    }
}
