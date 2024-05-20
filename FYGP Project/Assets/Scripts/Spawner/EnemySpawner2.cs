using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner2 : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRate = 0.8f;
    [SerializeField] private float coolDownRate = 10f;
    [SerializeField] private int safeDistance = 10; // Safe distance from the player
    [SerializeField] private int xPos = 10; // Range for spawning on the X-axis
    [SerializeField] private int zPos = 10; // Range for spawning on the Z-axis
    [SerializeField] public int enemyTotalCount = 0;
    [SerializeField] public int enemyTotalMaxCount = 100;
    [SerializeField] public int enemyWaveCount = 20;
    [SerializeField] public int wavePenalty = 6;

    private GameObject player;
    private int enemyCount = 0;
    private int waveCount = 0; // Number of waves spawned

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ESpawner());
    }

    private bool IsSafeSpawnPosition(Vector3 spawnPosition)
    {
        if (player == null)
            return true;

        float distanceToPlayer = Vector3.Distance(player.transform.position, spawnPosition);
        return distanceToPlayer >= safeDistance;
    }

    private bool IsOkayToSpawn(int enemyCount)
    {
        if (enemyTotalMaxCount <= enemyCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator ESpawner()
    {
        WaitForSeconds waitTime = new WaitForSeconds(spawnRate);
        WaitForSeconds cdTime = new WaitForSeconds(coolDownRate);

        IsOkayToSpawn(enemyTotalCount);

        if (true)
        {
            while (enemyCount <= enemyWaveCount)
            {
                yield return waitTime;

                int random = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyToSpawn = enemyPrefabs[random];

                Vector3 spawnPosition;
                bool safeToSpawn;

                #region Spawn Place
                do
                {
                    int randomXpos = Random.Range(-xPos, xPos) + (int)transform.position.x;
                    int randomZpos = Random.Range(-zPos, zPos) + (int)transform.position.z;
                    spawnPosition = new Vector3(randomXpos, transform.position.y, randomZpos);

                    safeToSpawn = IsSafeSpawnPosition(spawnPosition);
                }
                while (!safeToSpawn);
                #endregion

                if (enemyCount == enemyWaveCount)
                {
                    waveCount++;

                    if (waveCount >= wavePenalty && coolDownRate > 0.5f)
                    {
                        coolDownRate -= 0.5f;
                    }
                    cdTime = new WaitForSeconds(coolDownRate > 0.5f ? coolDownRate : 0.5f);

                    yield return cdTime;
                    enemyCount = 0;
                }

                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                enemyCount++;
                enemyTotalCount++;
            }
        }
    }
}
