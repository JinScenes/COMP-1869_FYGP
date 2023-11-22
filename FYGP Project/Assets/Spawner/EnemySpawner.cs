using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRate = 0.8f;
    [SerializeField] private float coolDownRate = 2f;
    [SerializeField] private int xPos = 0;
    [SerializeField] private int zPos = 0;
    [SerializeField] public int enemyMaxCount = 20;

    protected int randomXpos;
    protected int randomZpos;
    protected int enemyCount = 0;


    void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        // Time for new enmey spawn
        WaitForSeconds waitTime = new WaitForSeconds (spawnRate);
        // Time for reseting spawn
        WaitForSeconds cdTime = new WaitForSeconds (coolDownRate); 


        while (enemyCount <= enemyMaxCount)
        {
            // Time between each spawn
            yield return waitTime;

            // Spawn a random enemy from assigned prefabs
            int random = Random.Range (0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[random];

            //Setting random location

            randomXpos = Random.Range(0, xPos);
            randomZpos = Random.Range(0, zPos);

            Instantiate(enemyToSpawn, new Vector3(randomXpos, 0, randomZpos), Quaternion.identity);

            enemyCount++;


            if (enemyCount == enemyMaxCount)
            {
                print("Goint to reset");
                yield return cdTime;
                print("resetting");
                enemyCount = 0;
            }
        }

    } 
}
