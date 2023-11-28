using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceLightDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Light>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<Light>().enabled = false;
    }
}