using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunBase : MonoBehaviour
{

    [SerializeField] private GameObject reloadUIPrefab;
    #region Variables
    [SerializeField] Inventory inventory;
    public gunHolder GunHolder;
    private GunData previousGunData;
    public enum FireMode { Hitscan, Projectile, Cone }

    [SerializeField] FireMode fireMode;
    public Transform firePoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform gunSpawn;

    [Tooltip("Configurable Variables"),Space(5)]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float FireRate = 1f;
    [SerializeField] float range = 100f;
    [SerializeField] int MaxAmmo;
    [SerializeField] float reloadTime = 1.5f;

    public PlayerStats playerStats;
    [SerializeField] bool allowFire;
    public int currentAmmo;
    [SerializeField] bool isReloading = false;
    [SerializeField] float nextFireTime = 0f;
    private Animator animator;
    private AmmoType currentAmmoType;
    
    public  GunData NewData;
    #endregion

    #region Generic Functions


    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        playerStats = gameObject.GetComponentInParent<PlayerStatsHandler>().playerStats;
        //currentAmmo = MaxAmmo;
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
        /*if(NewData == null)
        {
            return;
        }*/

        if (currentAmmo <= 0)
        {
            isReloading = true;
            Debug.Log("Reloading...");
            StartCoroutine(Reload());

            return;
        }
        /*if (fireMode == FireMode.Hitscan)
        {
            ShootHitscan();
        }*/
        if (fireMode == FireMode.Projectile && Time.time >= nextFireTime && !isReloading)
        {
            nextFireTime = Time.time + 1f / FireRate;
            LaunchProjectile();
            currentAmmo--;
        } 

        if(fireMode == FireMode.Cone && Time.time >= nextFireTime && !isReloading)
        {
            nextFireTime = Time.time + 1f / FireRate;
            ShotgunFire();
            currentAmmo--;
        }

        
        
        
    }

    IEnumerator Reload()
    {
        
        Vector3 reloadUIPosition = gameObject.transform.parent.position+ new Vector3(0, 2, 0);

        
        GameObject reloadUIInstance = Instantiate(reloadUIPrefab, reloadUIPosition, Quaternion.identity);
        reloadUIInstance.transform.SetParent(this.transform); // To make sure it follows the player if they move

        Slider reloadSlider = reloadUIInstance.GetComponentInChildren<Slider>(); // Assuming the Slider component is a child

        

        Debug.Log("Reloading...");
        float startTime = Time.time;

        while (Time.time - startTime < reloadTime)
        {
            float progress = (Time.time - startTime) / reloadTime;
            reloadSlider.value = progress;
            yield return null;
        }

        reloadSlider.value = 1f; 

       
        currentAmmo = MaxAmmo;
        isReloading = false;

        // Destroy the reload UI
        Destroy(reloadUIInstance);
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

        

    }
    #endregion

    #region Projectile Code
    public void LaunchProjectile()
    {

        int ammoCount = 0;
        switch (currentAmmoType)
        {
            case AmmoType.SmallAmmo:
                ammoCount = playerStats.playerAmmo.smallAmmo.ammount;
                break;
            case AmmoType.MediumAmmo:
                ammoCount = playerStats.playerAmmo.mediumAmmo.ammount;
                break;
            case AmmoType.LargeAmmo:
                ammoCount = playerStats.playerAmmo.largeAmmo.ammount;
                break;
            default:
                print("Ammo type not found");
                return; 
        }
        if (ammoCount <= 0)
        {
            Debug.Log("Out of Ammo!");
            return;
        }

        switch (currentAmmoType)
        {
            case AmmoType.SmallAmmo:
                playerStats.playerAmmo.smallAmmo.ammount--;
                break;
            case AmmoType.MediumAmmo:
                playerStats.playerAmmo.mediumAmmo.ammount--;
                break;
            case AmmoType.LargeAmmo:
                playerStats.playerAmmo.largeAmmo.ammount--;
                break;
            default:
                print("Ammo type not found");
                break;
        }
        
        
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
        
        playerStats.UIHandle.UpdateAllAmmo(playerStats.playerAmmo);
    }

    private void ShotgunFire()
    {
        Debug.Log("Shotgun initialised");
        int ammoCount = 0;
        switch (currentAmmoType)
        {
            case AmmoType.SmallAmmo:
                ammoCount = playerStats.playerAmmo.smallAmmo.ammount;
                break;
            case AmmoType.MediumAmmo:
                ammoCount = playerStats.playerAmmo.mediumAmmo.ammount;
                break;
            case AmmoType.LargeAmmo:
                ammoCount = playerStats.playerAmmo.largeAmmo.ammount;
                break;
            default:
                print("Ammo type not found");
                return;
        }
        if (ammoCount <= 0)
        {
            Debug.Log("Out of Ammo!");
            return;
        }

        switch (currentAmmoType)
        {
            case AmmoType.SmallAmmo:
                playerStats.playerAmmo.smallAmmo.ammount--;
                break;
            case AmmoType.MediumAmmo:
                playerStats.playerAmmo.mediumAmmo.ammount--;
                break;
            case AmmoType.LargeAmmo:
                playerStats.playerAmmo.largeAmmo.ammount--;
                break;
            default:
                print("Ammo type not found");
                break;
        }

        int pelletsPerShot = 6; 
        float spreadAngle = 2f; 

        for (int i = 0; i < pelletsPerShot; i++)
        {
            
            Quaternion pelletRotation = Quaternion.Euler(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0) * firePoint.rotation;

            
            GameObject pellet = Instantiate(projectilePrefab, firePoint.position, pelletRotation);

            
            Rigidbody rb = pellet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = pellet.transform.forward * projectileSpeed;
            }
            playerStats.UIHandle.UpdateAllAmmo(playerStats.playerAmmo);
        }

        Debug.Log("Shotgun fired");
    }
    #endregion

    #region Scriptable object Loading

    public GameObject GunModel;
     public GameObject instGun;
    
     //AnimationClip AnimFire,AnimReload;

    public GameObject Initialize(GunData gunData)
    {
        animator.SetBool("isShooting", true);
        projectilePrefab = gunData.projectile;
        GunModel = gunData.gunModel;
        reloadTime = gunData.reloadTime;
        MaxAmmo = gunData.maxAmmo;
        FireRate = gunData.firerate;
        if (gunData.isCone == true)
        {
            fireMode = FireMode.Cone;
        } else { fireMode = FireMode.Projectile; }
        //AnimFire = gunData.fire;
        //AnimReload = gunData.reload;
       
        print(GunModel);
        
        print(MaxAmmo.ToString());
        print(FireRate.ToString());

        instGun = Instantiate(GunModel, gunSpawn.position, gunSpawn.rotation);
        instGun.transform.SetParent(gunSpawn);
        instGun.transform.Rotate(new Vector3(0f, 180f, 0f));
        currentAmmoType = gunData.type;
        print(currentAmmoType);
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


