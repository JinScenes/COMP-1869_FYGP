using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    #region Variables
    [SerializeField] Inventory inventory;
    public gunHolder GunHolder;
    private GunData previousGunData;
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

    public PlayerStats playerStats;
    [SerializeField] bool allowFire;
    protected int currentAmmo;
    [SerializeField] bool isReloading = false;
    [SerializeField] float nextFireTime = 0f;

    public  GunData NewData;
    #endregion

    #region Generic Functions


    private void Start()
    {
        /*inventory = gameObject.GetComponent<Inventory>();
        currentAmmo = MaxAmmo;
        if (NewData != null)
        {
            Initialize(NewData);
        }
*/
    }

    private void Update()
    {
        //GunData currentGun = playerStats.EquippedGun;
       
        /*if (GunHolder.CurrentGunData != previousGunData)
        {
            EquipGun(GunHolder.CurrentGunData);
            previousGunData = GunHolder.CurrentGunData;
        }*/
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

    public GameObject GunModel;
     public GameObject instGun;
    
     AnimationClip AnimFire,AnimReload;

    public GameObject Initialize(GunData gunData)
    {
        GunModel = gunData.gunModel;
        projectilePrefab = gunData.projectile;
        MaxAmmo = gunData.maxAmmo;
        FireRate = gunData.firerate;
        AnimFire = gunData.fire;
        AnimReload = gunData.reload;

        print(GunModel);
        
        print(MaxAmmo.ToString());
        print(FireRate.ToString());

        instGun = Instantiate(GunModel, gunSpawn.position, gunSpawn.rotation);
        instGun.transform.SetParent(gunSpawn);
        instGun.transform.Rotate(new Vector3(0f, 180f, 0f));

        return instGun;


    }

    /*public void EquipGun(GunData gunData)
    {
        Debug.Log(gunData);
        NewData = gunData;
        (GunData oldGunData, GameObject oldGunGameObject) = GunHolder.SwapGun(NewData);  // Call SwapGun method and get the old gun data and object

        if (oldGunData != null && inventory.Add(oldGunData))  // Check if oldGunData is not null and if it can be added to the inventory
        {
            // Successfully added the old gun back to the inventory
        }
        else
        {
            Debug.LogWarning("Failed to add the old gun back to the inventory");
        }
        // Additional code to update the UI or other gameplay elements if needed
    }*/

    /*public GunData UnequipGun()
    {
        GunData unequippedGun = NewData;
        NewData = null;
        return unequippedGun;
    }*/


    #endregion

    #region Damage

    #endregion

}


