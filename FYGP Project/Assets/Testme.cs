using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Testme : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI te = GetComponent<TextMeshProUGUI>();
        te.text = "EEEE"; 
        Debug.Log(te);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
