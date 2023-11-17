using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extraction : MonoBehaviour
{
    public GameObject flare, extractionSpawners;
    private HashSet<GameObject> playersInTrigger = new HashSet<GameObject>();
    private int requiredPlayers = 2; // Set this to the number of players required for extraction

    // Start is called before the first frame update
    void Start()
    {
        flare.SetActive(false);
        extractionSpawners.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Add the player to the HashSet
            playersInTrigger.Add(other.gameObject);

            // Check if the required number of players are in the trigger
            if (playersInTrigger.Count == requiredPlayers)
            {
                flare.SetActive(true);
                Extracting();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Remove the player from the HashSet
            flare.SetActive(false);

            playersInTrigger.Remove(other.gameObject);
        }
    }

    public void Extracting()
    {
        // Extraction logic here
    }
}
