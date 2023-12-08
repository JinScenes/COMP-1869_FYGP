using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tardis : MonoBehaviour
{
    public GameObject tpOrigin;
    // Start is called before the first frame update
    private HashSet<GameObject> playersInTrigger = new HashSet<GameObject>();
    public int requiredPlayers = 0;
    private Coroutine extractionCoroutine;
    public float extractionDuration = 3f;
    public TextMeshProUGUI extractionText;


    void Start()
    {
        extractionText.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playersInTrigger.Add(other.gameObject);

            if (playersInTrigger.Count == requiredPlayers && extractionCoroutine == null)
            {
                extractionCoroutine = StartCoroutine(ExtractionCoroutine());
                extractionText.enabled = true;
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
                extractionText.enabled = false;
            }
        }
    }

    private IEnumerator ExtractionCoroutine()
    {

        float timeRemaining = extractionDuration;
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);

            if (playersInTrigger.Count < requiredPlayers)
            {
                // Not enough players, abort extraction
                yield break;
            }
            timeRemaining -= 1f;
            extractionText.text = "Returning Home in: " + timeRemaining;
            //Debug.Log(timeRemaining);

        }
        Extracting(); // this could be the shop stuff maybe? 
    }

    private void Extracting()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = tpOrigin.transform.position;
        foreach (GameObject pl in players)
        {
            pl.transform.position = tpOrigin.transform.position;
        }

        Debug.Log("Extraction Complete!");
        Destroy(this.gameObject);
    }
}
