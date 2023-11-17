using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extraction : MonoBehaviour
{
    public GameObject flare; // extractionSpawners
    private HashSet<GameObject> playersInTrigger = new HashSet<GameObject>();
    private int requiredPlayers = 2; // I needa make this dynamic 💀
    private Coroutine extractionCoroutine;
    public float extractionDuration = 20f; 

    void Start()
    {
        flare.SetActive(false);
        //extractionSpawners.SetActive(false);
    }

    private void FixedUpdate()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playersInTrigger.Add(other.gameObject);

            if (playersInTrigger.Count == requiredPlayers && extractionCoroutine == null)
            {
                extractionCoroutine = StartCoroutine(ExtractionCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playersInTrigger.Remove(other.gameObject);
            if (playersInTrigger.Count < requiredPlayers && extractionCoroutine != null)
            {
                StopCoroutine(extractionCoroutine);
                extractionCoroutine = null;
                flare.SetActive(false);
                //extractionSpawners.SetActive(true);
            }
        }
    }

    private IEnumerator ExtractionCoroutine()
    {
        flare.SetActive(true);
        //extractionSpawners.SetActive(true);

        float timeRemaining = extractionDuration;
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            if (playersInTrigger.Count < requiredPlayers)
            {
                // Not enough players, abort extraction
                flare.SetActive(false);
                //extractionSpawners.SetActive(false);
                yield break;
            }
            timeRemaining -= 1f;
            Debug.Log(timeRemaining);
        }
        Extracting(); // this could be the shop stuff maybe? 
    }

    private void Extracting()
    {
        Debug.Log("Extraction Complete!");
    }
}
