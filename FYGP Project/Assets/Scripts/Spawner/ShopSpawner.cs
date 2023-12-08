using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] lootPrefab;
    [SerializeField] private int xPos = 0;
    [SerializeField] private int zPos = 0;
    [SerializeField] public int totalLoots = 12;
    [SerializeField] private float safeDistance = 2.0f; // Minimum distance between each loot


    private List<Vector3> spawnedPositions = new List<Vector3>();
    protected int lootCount = 0;
    protected const float lootSpawnTime = 0.2f;

    void Start()
    {
        StartCoroutine(S_Spawner());
    }

    private IEnumerator S_Spawner()
    {

        while (lootCount < totalLoots)
        {
            yield return new WaitForSeconds(lootSpawnTime);

            Vector3 spawnPosition;
            bool isPositionSafe;

            do
            {
                int randomXpos = Random.Range(-xPos, xPos) + (int)transform.position.x;
                int randomZpos = Random.Range(-zPos, zPos) + (int)transform.position.z;
                spawnPosition = new Vector3(randomXpos, transform.position.y, randomZpos);

                isPositionSafe = IsSafeSpawnPosition(spawnPosition);
            }
            while (!isPositionSafe);

            Instantiate(lootPrefab[Random.Range(0, lootPrefab.Length)], spawnPosition, Quaternion.Euler(-90f, 0f, 0f));
            spawnedPositions.Add(spawnPosition);
            lootCount++;
        }

        //Destroy(gameObject);
    }

    private bool IsSafeSpawnPosition(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < safeDistance)
            {
                return false;
            }
        }
        return true;
    }
}
