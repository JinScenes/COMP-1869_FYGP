using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    [Tooltip("Unique identifier for a pair of pressure plates. Pressure plates with the same ID form a pair.")]
    public string pairID;
    public GameObject cDoor1, cDoor2, oDoor1, oDoor2;

    [SerializeField] private float initialTimeFrame = 2.0f;

    private static Dictionary<string, int> activatedPlates = new Dictionary<string, int>();
    private static Dictionary<string, Coroutine> activationTimers = new Dictionary<string, Coroutine>();

    public static bool ActionOn { get; private set; }

    private void Start()
    {
        if (string.IsNullOrEmpty(pairID))
        {
            Debug.LogWarning("You need to put a number for PairID");
        }
    }

    private void FixedUpdate()
    {
        if (ActionOn == true)
        {
            powerOn();
            ActionOn = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            if (!activatedPlates.ContainsKey(pairID))
            {
                activatedPlates[pairID] = 0;
            }
            activatedPlates[pairID]++;
            ManageActivationTimer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null && activatedPlates.ContainsKey(pairID))
        {
            activatedPlates[pairID]--;
            ManageActivationTimer();
        }
    }

    private void ManageActivationTimer()
    {
        float adjustedTimeFrame = initialTimeFrame / (GamepadInputManager.currentPlayerCount + 1);

        if (activatedPlates[pairID] == 1)
        {
            if (activationTimers.ContainsKey(pairID))
            {
                StopCoroutineSafe(pairID);
            }
            activationTimers[pairID] = StartCoroutine(ActivationTimer(pairID, adjustedTimeFrame));
        }
        else if (activatedPlates[pairID] == 2)
        {
            StopCoroutineSafe(pairID);
        }

        CheckPairs();
    }

    private IEnumerator ActivationTimer(string id, float timeFrame)
    {
        Debug.Log($"Starting timer for {id} with time frame: {timeFrame}");
        yield return new WaitForSeconds(timeFrame);

        if (activatedPlates.ContainsKey(id) && activatedPlates[id] < 2)
        {
            Debug.Log($"Timer expired for {id}, resetting activation.");
            activatedPlates[id] = 0;
            activationTimers[id] = null;
        }

        CheckPairs();
    }

    private void StopCoroutineSafe(string id)
    {
        if (activationTimers[id] != null)
        {
            Debug.Log($"Stopping timer for {id}");
            StopCoroutine(activationTimers[id]);
            activationTimers[id] = null;
        }
    }

    private static void CheckPairs()
    {
        foreach (var pair in activatedPlates)
        {
            if (pair.Value < 2)
            {
                ActionOn = false;
                Debug.Log("Not all pairs are activated.");
                return;
            }
        }

        Debug.Log("All pairs activated.");
        ActionOn = true;
    }

    private void powerOn()
    {
        cDoor1.SetActive(false);
        cDoor2.SetActive(false);
        oDoor1.SetActive(true);
        oDoor2.SetActive(true);

        GameObject[] lights = GameObject.FindGameObjectsWithTag("StreetLight");
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }
    }
}
