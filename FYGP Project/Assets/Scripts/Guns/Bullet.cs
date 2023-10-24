using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bDamage;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            EnemyFSM enemyFSM = other.gameObject.GetComponent<EnemyFSM>();

            if (enemyFSM.healthModule.IsDead())
            {
                Debug.Log("dead");
                return;
            }
            else
            {
                Debug.Log("EnemyHit");
                enemyFSM.healthModule.EnemyDamage(bDamage);
            }
        }
    }
}
