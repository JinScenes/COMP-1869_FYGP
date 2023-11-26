using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] lootPrefab;
    [SerializeField] private GameObject[] buildingPrefab;
    [SerializeField] private float xPos = 0;
    [SerializeField] private float zPos = 0;
    [SerializeField] private float buildingPossiblity = 0.6f;
    [SerializeField] public int totalLoots = 20;
    
    protected float randomXpos;
    protected float randomZpos;
    
    protected const float lootSpawnTime = 0.2f; 
    protected const float minDistanceBetweenLoots = 5f; 

    private List<Vector3> spawnedLootPositions = new List<Vector3>();


    void Start()
    {
        StartCoroutine(L_Spawner());
    }

    private IEnumerator L_Spawner()
    {
        yield return new WaitForSeconds(lootSpawnTime);

        while (spawnedLootPositions.Count < totalLoots)
        {
            GameObject lootToSpawn = GetRandomLoot();

            bool positionValid = false;
            Vector3 spawnPosition = Vector3.zero;

            // Try to find a valid position for loot
            while (!positionValid)
            {
                randomXpos = Random.Range(-xPos, xPos);
                randomZpos = Random.Range(-zPos, zPos);
                spawnPosition = new Vector3(randomXpos, 0.5f, randomZpos);

                // Check distance with existing loot positions
                positionValid = IsPositionValid(spawnPosition);
            }

            // Check if the loot is inside an enterable building based on the probability
            if (Random.value < buildingPossiblity)
            {
                // Spawn inside a building

                GameObject building = GetBuilding();
                spawnPosition = GetPosition(building);
            }

            // Instantiate loot at the valid position
            Instantiate(lootToSpawn, spawnPosition, Quaternion.identity);
            spawnedLootPositions.Add(spawnPosition);
        }
    }

    private GameObject GetRandomLoot()
    {
        return lootPrefab[Random.Range(0, lootPrefab.Length)];
    }

    private GameObject GetBuilding()
    {
        return buildingPrefab[Random.Range(0, buildingPrefab.Length)];
    }

    private Vector3 GetPosition(GameObject building)
    {
        return building.transform.position;
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 existingPosition in spawnedLootPositions)
        {
            float distance = Vector3.Distance(position, existingPosition);
            if (distance < minDistanceBetweenLoots)
            {
                return false;
            }
        }
        return true;
    }
}
