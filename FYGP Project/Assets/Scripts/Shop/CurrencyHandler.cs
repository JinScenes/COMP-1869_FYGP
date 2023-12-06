using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] int currencyPerEnemy = 10;
    int totalMoney;
    //public int currencyPerEnemy = 10;

    void Awake()
    {
        totalMoney = 0;
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
            //.Find("txtAmount").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        moneyText.text = totalMoney.ToString();
    }

    // Method when an enemy is defeated
    public void OnEnemyDefeated()
    {
        totalMoney += currencyPerEnemy;
    }

    //// Method to spend currency in the shop
    //public bool TrySpendCurrency(int amount)
    //{
    //    if (PlayerStats.currency >= amount)
    //    {
    //        PlayerStats.currency -= amount;
    //        UpdateCurrencyUI();
    //        return true; 
    //    }

    //    return false; // Not enough currency
    //}
}
