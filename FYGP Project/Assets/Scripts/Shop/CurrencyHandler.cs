using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyHandler : MonoBehaviour
{
    public PlayerStatsHandler playerStatsHandler;
    public int currencyPerEnemy = 10;
    public GameObject enemies;

    void Awake()
    {
        // Initialize or link to PlayerStatsHandler, if necessary
        // For example, you might find it like this if it's attached to the same GameObject:
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
    }

    private void Update()
    {
        // enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //if 
    }

    // Method when an enemy is defeated
    public void OnEnemyDefeated()
    {
        PlayerStats.currency += currencyPerEnemy;
        UpdateCurrencyUI();
    }

    // Method to spend currency in the shop
    public bool TrySpendCurrency(int amount)
    {
        if (PlayerStats.currency >= amount)
        {
            PlayerStats.currency -= amount;
            UpdateCurrencyUI();
            return true; 
        }

        return false; // Not enough currency
    }

    // Update the UI to reflect the current currency amount
    private void UpdateCurrencyUI()
    {
        // Assuming you have a method in PlayerStatsHandler or somewhere similar to update the UI
        //playerStatsHandler.UpdateCurrencyUI(PlayerStats.currency);
    }
}
