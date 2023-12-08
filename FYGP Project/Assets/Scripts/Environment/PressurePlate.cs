using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    public GameObject cDoor1, cDoor2, oDoor1, oDoor2;
    [Space(20)] public string pairID;

    [SerializeField] private float initialTimeFrame = 2.0f;

    [SerializeField] private Material activatedMaterial;
    [SerializeField] private Material deactivatedMaterial;

    private static Dictionary<string, int> activatedPlates = new Dictionary<string, int>();
    private static Dictionary<string, Coroutine> activationTimers = new Dictionary<string, Coroutine>();

    private bool isDeactivationDelayed = false;
    private float deactivationDelay = 0.5f;

    private GamepadInputManager gamepadInput;
    private Renderer plateRenderer;

    public static bool ActionOn { get; private set; }

    private void Awake()
    {
        gamepadInput = FindObjectOfType<GamepadInputManager>();
    }

    private void Start()
    {

        if (string.IsNullOrEmpty(pairID))
        {
            Debug.LogWarning("Pressure Plate requires a non-empty pairID.");
        }

        plateRenderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (ActionOn == true)
        {
            PowerOn();
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
            Debug.Log("Plate " + pairID + " activated. Current count: " + activatedPlates[pairID]);
            ManageActivationTimer();
            plateRenderer.material = activatedMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null && activatedPlates.ContainsKey(pairID))
        {
            if (!isDeactivationDelayed)
            {
                StartCoroutine(DelayDeactivation(pairID));
            }
        }
    }

    private void ManageActivationTimer()
    {
        int playerCount = Mathf.Max(1, gamepadInput.currentPlayerCount);
        float adjustedTimeFrame = initialTimeFrame / playerCount;

        if (activatedPlates[pairID] == 1)
        {
            if (activationTimers.ContainsKey(pairID) && activationTimers[pairID] != null)
            {
                StopCoroutine(activationTimers[pairID]);
            }
            activationTimers[pairID] = StartCoroutine(ActivationTimer(pairID, adjustedTimeFrame));
        }
        else if (activatedPlates[pairID] == 2)
        {
            StopCoroutineSafe(pairID);
        }

        CheckPairs();
    }

    private IEnumerator DelayDeactivation(string id)
    {
        RevertColor();
        isDeactivationDelayed = true;
        yield return new WaitForSeconds(deactivationDelay);
        if (activatedPlates[id] > 0)
        {
            activatedPlates[id]--;
        }
        Debug.Log("Plate " + id + " deactivated after delay. Current count: " + activatedPlates[id]);
        ManageActivationTimer();
        isDeactivationDelayed = false;
    }

    private IEnumerator ActivationTimer(string id, float timeFrame)
    {
        yield return new WaitForSeconds(timeFrame);

        if (activatedPlates.ContainsKey(id) && activatedPlates[id] < 2)
        {
            activatedPlates[id] = 0;
            activationTimers[id] = null;
            CheckPairs();
            plateRenderer.material = deactivatedMaterial;
        }
    }

    private void StopCoroutineSafe(string id)
    {
        if (activationTimers.ContainsKey(id) && activationTimers[id] != null)
        {
            StopCoroutine(activationTimers[id]);
            activationTimers[id] = null;
        }
    }

    private static void CheckPairs()
    {
        ActionOn = true;
        foreach (var pair in activatedPlates)
        {
            if (pair.Value < 2)
            {
                Debug.Log("pair value is " + pair.Value);
                ActionOn = false;
                break;
            }
        }

        Debug.Log(ActionOn ? "All pairs activated." : "Not all pairs are activated.");
    }

    private void RevertColor()
    {
        if (plateRenderer != null)
        {
            plateRenderer.material = deactivatedMaterial;
        }
    }

    private void PowerOn()
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
