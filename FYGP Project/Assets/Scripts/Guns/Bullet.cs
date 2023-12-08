using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float maxDistance = 100f;
    //[SerializeField] private float radius; // Radius for SphereCollider
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        // Add a SphereCollider for proximity detection
        var collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        //collider.radius = radius;

    }

    void Update()
    {
        float distanceThisFrame = speed * Time.deltaTime;
        RaycastCheck(distanceThisFrame);
        transform.Translate(Vector3.forward * distanceThisFrame);

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void RaycastCheck(float distanceThisFrame)
    {
        Debug.DrawRay(transform.position, transform.forward * distanceThisFrame, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distanceThisFrame))
        {
            HitDetectedR(hit);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitDetectedC(other);
        }
    }

    private void HitDetectedR(RaycastHit hit)
    {
        var enemy = hit.transform.GetComponent<EnemyFSM>();
        if (enemy != null)
        {
            Debug.Log("Enemy Hit by Raycast");
            enemy.healthModule.EnemyDamage(damage);
        }
        Destroy(gameObject);
    }

    private void HitDetectedC(Collider other)
    {
        var enemy = other.GetComponent<EnemyFSM>();
        if (enemy != null)
        {
            Debug.Log("Enemy Hit by SphereCollider");
            enemy.healthModule.EnemyDamage(damage);
        }
        Destroy(gameObject);
    }
}
