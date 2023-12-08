using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Extraction : MonoBehaviour
{
    public GameObject flare, tpLoc; 
    private HashSet<GameObject> playersInTrigger = new HashSet<GameObject>();
    public int requiredPlayers = 0; 
    private Coroutine extractionCoroutine;
    public float extractionDuration = 20f;
    public GameObject[] extractionSpawners;
    public TextMeshProUGUI extractionText;
    public bool isFinalExtractor = false;

    void Start()
    {
        extractionText.enabled = false;
        flare.SetActive(false);
        foreach (GameObject spawner in extractionSpawners)
        {
            spawner.SetActive(false);
        }
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
                flare.SetActive(false);
                extractionText.enabled = false;
                foreach (GameObject spawner in extractionSpawners)
                {
                    spawner.SetActive(true);
                }
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
            // Enables the spawners so more zombies spawn while extracting
            foreach (GameObject spawner in extractionSpawners)
            {
                spawner.SetActive(true);
            }

            if (playersInTrigger.Count < requiredPlayers)
            {
                // Not enough players, abort extraction
                flare.SetActive(false);
                //extractionSpawners.SetActive(false);
                yield break;
            }
            timeRemaining -= 1f;
            extractionText.text = "Extraction Complete in: " + timeRemaining;
            Debug.Log(timeRemaining);

        }
        Extracting(); // this could be the shop stuff maybe? 
    }

    private void Extracting()
    {
        if (isFinalExtractor == true)
        {
            SceneManager.LoadScene("EndScreen");
        }
        Debug.Log("Extraction Complete!");
        Destroy(this.gameObject);
        foreach (GameObject spawner in extractionSpawners)
        {
            // TP Camera & Players
            //GameObject.FindGameObjectWithTag("MainCamera").transform.position = ;
            spawner.SetActive(false);
        }
        
    }
}
