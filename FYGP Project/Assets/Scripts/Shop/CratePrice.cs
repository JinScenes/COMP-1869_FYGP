using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CratePrice : MonoBehaviour
{
    [SerializeField] private TextMeshPro txtPrice;

    private LockCrate lockCrate;


    private void Start()
    {
        lockCrate = GetComponent<LockCrate>();

        
        txtPrice.text = "Click Y To Purchase:\n" + lockCrate.cratePrice.ToString();
    }
}
