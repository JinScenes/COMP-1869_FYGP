using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage;
    private float timer =4f;
    

    // Update is called once per frame
    void Update()
    {
        timer = Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
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
                Destroy(gameObject);
            }
        }


        Destroy(gameObject);
    }
}
