using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRange = 5.0f;
    [SerializeField] private float spawnRate = 0.8f;
    [SerializeField] private float coolDownRate = 10f;
    [SerializeField] private int xPos = 0;
    [SerializeField] private int zPos = 0;
    [SerializeField] public int enemyMaxCount = 20;
    [SerializeField] public int wavePenalty = 6; // when wave reach the set time (reduce the coolDownRate)

    [SerializeField] public bool autoOff = false;
    [SerializeField] public int finishWave = 2;

    private GameObject player;
    protected int randomXpos;
    protected int randomZpos;
    protected int enemyCount = 0;
    protected int waveCount = 0; // wave have spawn
    protected float distanceToPlayer;

    void Start()
    {
        waveCount = 0;
        StartCoroutine(E_Spawner());
    }

    private void FindDistance()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, new Vector3(randomXpos, 0, randomZpos));
        }

    }

    private IEnumerator E_Spawner()
    {
        // Time for new enmey spawn
        WaitForSeconds waitTime = new WaitForSeconds (spawnRate);
        // Time for reseting spawn
        WaitForSeconds cdTime = new WaitForSeconds (coolDownRate);

        int waveToFinish = wavePenalty + finishWave;

        while (enemyCount <= enemyMaxCount)
        {

            // Time between each spawn
            yield return waitTime;

            // Spawn a random enemy from assigned prefabs
            int random = Random.Range (0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[random];

            // Check the distance of player
            FindDistance();

            if (distanceToPlayer >= spawnRange)
            {
                // Setting random location
                randomXpos = Random.Range(-xPos, xPos);
                randomZpos = Random.Range(-zPos, zPos);

                // Spawning
                Instantiate(enemyToSpawn, new Vector3(randomXpos, 0, randomZpos), Quaternion.identity);

                enemyCount++;

                if (enemyCount == enemyMaxCount)
                {
                    //Debug.Log("Wave" + waveCount + "start");
                    //Debug.Log(waveCount);
                    waveCount++;
                    
                    if (autoOff = true && waveCount >= waveToFinish)
                    {
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        if(waveCount >= wavePenalty)
                        {
                            //print("wave count >= wave penaly");
                            for(int i = 0; i < waveCount; i++)
                            {
                                //print("start looping for once");
                                if(coolDownRate <= 0.5f)
                                {
                                    cdTime = new WaitForSeconds(0.5f);
                                }
                                else
                                {
                                    if (i >= 1)
                                    {
                                        cdTime = new WaitForSeconds(coolDownRate -= 0.5f);
                                
                                        break;
                                    }
                                }
                            }
                        }
                    }


                    yield return cdTime;

                    enemyCount = 0;
                }

            }
        }
    } 
}
