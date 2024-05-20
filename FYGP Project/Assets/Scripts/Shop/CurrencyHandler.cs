using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    public int totalMoney;
    public int initialMoney = 500;

    void Awake()
    {
        totalMoney = initialMoney;
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        moneyText.text = totalMoney.ToString();
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
    }
       
}
