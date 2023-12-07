using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemNameUpdater : MonoBehaviour
{
    //private AmmoCollection _AmmoCollection;
    [SerializeField]private GameObject Title;
    [SerializeField]private TextMeshPro _TextMeshProUGUI;
    private void Start()
    {
        _TextMeshProUGUI.text = Title.name;     
    }
}
