using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    #region Variables


    public enum FireMode { Hitscan, Projectile }

    [SerializeField] FireMode fireMode = FireMode.Hitscan;
    public Transform firePoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform gunSpawn;

    [Tooltip("Configurable Variables"),Space(5)]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float FireRate = 1f;
    [SerializeField] float range = 100f;
    [SerializeField] int MaxAmmo = 30;
    [SerializeField] float reloadTime = 1.5f;


    [SerializeField] bool allowFire;
    protected int currentAmmo;
    [SerializeField] bool isReloading = false;
    [SerializeField] float nextFireTime = 0f;

    [SerializeField] GunData NewData;
    #endregion

    #region Generic Functions


    private void Start()
    {
        currentAmmo = MaxAmmo;
        Initialize(NewData);
    }

    private void Update()
    {
        

        /*if (currentAmmo <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                
            }
        }*/

       /* if (Input.GetKey(KeyCode.Mouse0) )
        {
            nextFireTime = Time.time + 1f / FireRate;
            Fire();
        }*/
    }

    public void Fire()
    {
        if (isReloading)
        {
            return;
        }

        if (fireMode == FireMode.Hitscan)
        {
            ShootHitscan();
        }
        else if (fireMode == FireMode.Projectile && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / FireRate;
            LaunchProjectile();
        } else if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;

        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = MaxAmmo;
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

     GameObject GunModel;
     
    
     AnimationClip AnimFire,AnimReload;

     void Initialize(GunData gunData)
    {
        GunModel = gunData.gunModel;
        
        MaxAmmo = gunData.maxAmmo;
        FireRate = gunData.firerate;
        AnimFire = gunData.fire;
        AnimReload = gunData.reload;

        print(GunModel);
        
        print(MaxAmmo.ToString());
        print(FireRate.ToString());

        GameObject instGun = Instantiate(GunModel, gunSpawn.position, Quaternion.identity);
        instGun.transform.SetParent(gunSpawn);
        instGun.transform.Rotate(new Vector3(0f, 180f,0f));


    }


    #endregion

    #region Damage
   
    #endregion

}


