using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    #region Variables
    public enum FireMode { Hitscan, Projectile }

    [SerializeField] FireMode fireMode = FireMode.Hitscan;
    public Transform firePoint;
    [SerializeField] GameObject projectilePrefab;


    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float fireRate = 0.2f;
    public float range = 100f;
    [SerializeField] int maxAmmo = 30;
    [SerializeField] float reloadTime = 1.5f;
    
    protected int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;
    #endregion

    #region Generic Functions


    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                StartCoroutine(Reload());
                return;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Fire();
        }
    }

    void Fire()
    {
        if (fireMode == FireMode.Hitscan)
        {
            ShootHitscan();
        }
        else if (fireMode == FireMode.Projectile)
        {
            LaunchProjectile();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded");
    }

    #endregion

    #region Hitscan Code
    protected virtual void ShootHitscan()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                // Handle hitscan hit logic here (e.g., deal damage, apply effects)
                print(hit.transform.name);
                Debug.DrawLine(ray.origin, hit.point, Color.red, 0.1f);
            }
        }
        currentAmmo--;
    }
    #endregion

    #region Projectile Code
    protected virtual void LaunchProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("ProjectilePrefab does not have a Rigidbody component.");
        }
        currentAmmo--;
    }
    #endregion

    #region Scriptable object Loading


   public class scriptableObjectLoader: MonoBehaviour
    {
        public GunData newGunData;
        

    }


    #endregion

}


