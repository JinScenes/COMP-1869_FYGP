using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Consumables : MonoBehaviour
{
    private GameObject waitActionObj;
    private GameObject waitActionSliderObj;
    private Transform waitActionDescription;

    private Inventory inventory;

    // The invdex of the item being consumed
    public int removeIndex;

    public bool doingAction = false;
    private string completedAction;

    private float cancelDistance = 1.2f;

    private PlayerController playerController;
    private MessageBillboard messageBillboard;


    // Start is called before the first frame update
    void Start()
    {
        // Make unique waitAction
        waitActionObj = (GameObject) GameObject.Instantiate(Resources.Load("WaitActionObj"), GameObject.Find("WaitActions").transform);

        // Set variables to make things eaiser
        waitActionSliderObj = FindChildGameObjectByName(waitActionObj.transform, "Slider").gameObject;
        waitActionDescription = FindChildGameObjectByName(waitActionObj.transform, "Description");

        inventory = GetComponent<PlayerStatsHandler>().playerInventory;
        playerController = GetComponent<PlayerController>();
        messageBillboard = GetComponent<MessageBillboard>();
    }

    public IEnumerator WaitAction(float waitTime, string desc, string afterWaitFunction)
    {
       
        doingAction = true;

        Vector3 startingPos = transform.position;

        float divisionRate = 50f;

        // Make success true if the item is instant use
        bool success = waitTime <= 0 ? true : false;

        AudioManager.instance.PlayAudios(afterWaitFunction + " Consume");


        if (!success)
        {
            waitActionObj.SetActive(true);
            waitActionDescription.GetComponent<TextMeshProUGUI>().text = desc;
            waitActionObj.transform.position = startingPos;

            WaitForSeconds incrementWaitTime = new WaitForSeconds(waitTime / divisionRate);

            for (int i = 0; i < (int)divisionRate; i++)
            {
                //print(Vector3.Distance(startingPos, transform.position));
                if (Vector3.Distance(startingPos, transform.position) >= cancelDistance || doingAction == false)
                {
                    messageBillboard.ShowMessage("Canceled due to moving / hit", 3f);
                    break;
                }

                // Update slider value
                waitActionSliderObj.GetComponent<Slider>().value = i / divisionRate;

                // wait for total time / division rate
                yield return incrementWaitTime;

                if (i == divisionRate - 1) { success = true; print("Sucessful consumption!"); }
            }

            waitActionObj.SetActive(false);
        }

        if (success == true)
        {
            print("Consumed " + afterWaitFunction);
            completedAction = afterWaitFunction;
            Invoke(afterWaitFunction, 0f);
        }

        doingAction = false;



    }

    private static Transform FindChildGameObjectByName(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name == name)
            {
                return parent.GetChild(i);
            }

            Transform child = FindChildGameObjectByName(parent.GetChild(i), name);

            if (child != null)
            {
                return child;
            }

        }

        return null;
    }

    private void AddHP(float amount)
    {
        playerController.AddHealth(amount);
    }

    private void ConsumeItem()
    {
        completedAction = "";
        inventory.RemoveFromIndex(removeIndex);
        inventory.consumingIndex = -1;
    }

    private float GetPrecentage(float value, float percentage)
    {
        return value/100 * percentage;
    }

    public void Medkit()
    {
        string methodName = MethodBase.GetCurrentMethod().Name;

        if (completedAction == methodName)
        {
            ConsumeItem();
            AddHP(1000);
            return;
        }

        if (doingAction == false)
        {
            StartCoroutine(WaitAction(5f, "I hope this doesn't take too long", methodName));
        }
    }

    public void Bandage()
    {
        string methodName = MethodBase.GetCurrentMethod().Name;

        if (completedAction == methodName)
        {
            ConsumeItem();
            AddHP(GetPrecentage(playerController.maxHealth, 15));
            return;
        }

        if (doingAction == false)
        {
            StartCoroutine(WaitAction(3f, "Wrap and roll ~", methodName));
        }

    }

    public void Syringe()
    {
        string methodName = MethodBase.GetCurrentMethod().Name;

        if (completedAction == methodName)
        {
            ConsumeItem();
            AddHP(GetPrecentage(playerController.maxHealth, 5));
            return;
        }

        if (doingAction == false)
        {
            StartCoroutine(WaitAction(0f, "Ouch!", methodName));
        }

    }

    public void SuperSyringe()
    {
        string methodName = MethodBase.GetCurrentMethod().Name;

        if (completedAction == methodName)
        {
            ConsumeItem();
            AddHP(GetPrecentage(playerController.maxHealth, 15));
            return;
        }

        if (doingAction == false)
        {
            StartCoroutine(WaitAction(0f, "This feels great!", methodName));
        }

    }

    public void BodyArmor()
    {
        string methodName = MethodBase.GetCurrentMethod().Name;

        if (completedAction == methodName)
        {
            ConsumeItem();
            playerController.IncreaseMaxHealth(20);
            return;
        }

        if (doingAction == false)
        {
            StartCoroutine(WaitAction(3f, "it's heavy...", methodName));
        }
    }

    public void RollerSkates()
    {
        string methodName = MethodBase.GetCurrentMethod().Name;

        if (completedAction == methodName)
        {
            ConsumeItem();
            playerController.speed += 2;
            return;
        }

        if (doingAction == false)
        {
            StartCoroutine(WaitAction(3f, "This is going to be awesome!", methodName));
        }
    }
}
