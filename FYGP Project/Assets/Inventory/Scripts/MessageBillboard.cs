using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MessageBillboard : MonoBehaviour
{
    public GameObject msgBillboard;

    private TextMeshProUGUI textUI;
    private bool showingMsg = false;

    private void Start()
    {
        textUI = msgBillboard.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    IEnumerator HandleMessage(string msg, float msgTime)
    {
        msgBillboard.SetActive(true);
        textUI.text = msg;

        yield return new WaitForSeconds(msgTime);

        msgBillboard.SetActive(false);
        textUI.text = string.Empty;
    }
    
    public void ShowMessage(string msg, float msgTime)
    {
        StopAllCoroutines();
        StartCoroutine(HandleMessage(msg, msgTime));
    }
}
