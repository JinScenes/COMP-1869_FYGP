using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float maxDistance = 100f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceThisFrame = speed * Time.deltaTime;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distanceThisFrame))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                var enemy = hit.transform.GetComponent<EnemyFSM>();
                if (enemy != null)
                {
                    Debug.Log("Enemy Hit");
                    enemy.healthModule.EnemyDamage(damage);
                }
            }
            Destroy(gameObject);
        }

        transform.Translate(Vector3.forward * distanceThisFrame);
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
