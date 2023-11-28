using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage;
<<<<<<< HEAD
=======
    private float timer =4f;
>>>>>>> BranchMerger
    

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        
=======
        timer = Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
>>>>>>> BranchMerger
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            
            GameObject enemy = other.gameObject;
            if (enemy.GetComponent<EnemyFSM>().health <= 0)
            {
                Debug.Log("dead");
                Destroy(gameObject);
                return;
            }
            else if(enemy.GetComponent<EnemyFSM>().health > 0)
            {
                Debug.Log("EnemyHit");
                enemy.GetComponent<EnemyFSM>().healthModule.EnemyDamage(damage);
<<<<<<< HEAD
=======
                Destroy(gameObject);
>>>>>>> BranchMerger
            }
        }


        Destroy(gameObject);
    }
}
