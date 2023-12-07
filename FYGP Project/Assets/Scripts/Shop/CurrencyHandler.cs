using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    public int totalMoney;

    void Awake()
    {
        totalMoney = 0;
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        moneyText.text = totalMoney.ToString();
        //UpdateMoneyText(totalMoney);
    }

    private void UpdateMoneyText(int amount)
    {
        moneyText.text = amount.ToString();
    }
       
}
