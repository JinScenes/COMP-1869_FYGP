using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    public int PlayerHealth = 5;
    public Text healthText;
    void Update()
    {
        healthText.text = ("HP: " + PlayerHealth).ToString();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayerHealth--;
        }
    }
}
