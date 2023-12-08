using Andtech.ProTracer;
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
    private bool gameStart= false;
    [SerializeField] FireMode fireMode;
    public Transform firePoint;
    [SerializeField] GameObject[] muzzleFlashPrefabs;
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
    int ammoCount = 0;
    public  GunData NewData;
    public float Speed => 10.0F + (tracerSpeed - 1) * 50.0F;
    public float RotationSpeed => 72.0F;
    public float TimeBetweenShots => 1.0F / FireRate;

    private GamepadInput gamepadInput;

    [Header("Prefabs")]
    [SerializeField]
    [Tooltip("The Bullet prefab to spawn.")]
    private ShotBullet bulletPrefab = default;
    [SerializeField]
    [Tooltip("The Smoke Trail prefab to spawn.")]
    private SmokeTrail smokeTrailPrefab = default;
    [Header("Demo Settings")]
    [SerializeField]
    [Tooltip("Rotate the spawn point?")]
    private bool spin = true;
    [Header("Raycast Settings")]
    [SerializeField]
    [Tooltip("The maximum raycast distance.")]
    private float maxQueryDistance = 300.0F;
    [Header("Tracer Settings")]
    [SerializeField]
    [Tooltip("The speed of the tracer graphics.")]
    [Range(1, 10)]
    private int tracerSpeed = 3;
    [SerializeField]
    [Tooltip("Should tracer graphics use gravity while moving?")]
    private bool useGravity = false;
    [SerializeField]
    [Tooltip("If enabled, a random offset is applied to the spawn point. (This eliminates the \"Wagon-Wheel\" effect)")]
    private bool applyStrobeOffset = true;

    string currentGunName;
    #endregion

    #region Generic Functions

    private void Awake()
    {
        gamepadInput = GetComponentInParent<GamepadInput>();
    }

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        playerStats = gameObject.GetComponentInParent<PlayerStatsHandler>().playerStats;
        StartInit(NewData);
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
        CheckAmmo();
        if (ammoCount > 0)
        {

            if (currentAmmo <= 0 && !isReloading)
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
                AudioManager.instance.PlayAudios("Assault Rifle Dry Shot");
                nextFireTime = Time.time + 1f / FireRate;
                LaunchProjectile();
                ShowMuzzleFlash();
                // currentAmmo--;
                gamepadInput.VibrateForDuration(0.75f, 0.75f, 0.1f);
                Debug.Log("Vibration called for Projectile Fire");
            }

            if (fireMode == FireMode.Cone && Time.time >= nextFireTime && !isReloading)
            {
                nextFireTime = Time.time + 1f / FireRate;
                ShotgunFire();
                ShowMuzzleFlash();
                //currentAmmo--;
                gamepadInput.VibrateForDuration(0.75f, 0.75f, 0.1f);
                Debug.Log("Vibration called for Projectile Fire");
            }


        }
        
    }

    IEnumerator Reload()
    {
        AudioManager.instance.PlayAudios("Assault Rifle Reload");
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

        //int ammoCount = 0;
        float speed = Speed;
        float offset;
        if (applyStrobeOffset)
        {
            offset = Random.Range(0.0F, CalculateStroboscopicOffset(speed));
        }
        else
        {
            offset = 0.0F;
        }

        // Instantiate the tracer graphics
        ShotBullet bullet = Instantiate(bulletPrefab);
        SmokeTrail smokeTrail = Instantiate(smokeTrailPrefab);

        print("Currently shooting: " + currentGunName);
        switch (currentGunName)
        {
            case "Pistol":
                bullet.GetComponent<Bullet>().damage = 20f;
                break;
            case "Rifle":
                bullet.GetComponent<Bullet>().damage = 25f;
                break;
            case "Smg":
                bullet.GetComponent<Bullet>().damage = 5f;
                break;
            case "Sniper":
                bullet.GetComponent<Bullet>().damage = 150f;
                break;
            default:
                break;
        }

        // Setup callbacks
        bullet.Completed += OnCompleted;
        smokeTrail.Completed += OnCompleted;

        // Use different tracer drawing methods depending on the raycast
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, maxQueryDistance))
        {
            // Since start and end point are known, use DrawLine
            bullet.DrawLine(transform.position, hitInfo.point, speed, offset);
            smokeTrail.DrawLine(transform.position, hitInfo.point, speed, offset);

            // Setup impact callback
            bullet.Arrived += OnImpact;

            void OnImpact(object sender, System.EventArgs e)
            {
                // Handle impact event here
                Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, 0.5F);
            }
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

        playerStats.UIHandle.UpdateAllAmmo(playerStats.playerAmmo);
    }

    private void ShowMuzzleFlash()
    {
        if (muzzleFlashPrefabs != null && muzzleFlashPrefabs.Length > 0)
        {
            int index = Random.Range(0, muzzleFlashPrefabs.Length);
            GameObject flash = Instantiate(muzzleFlashPrefabs[index], firePoint.position, firePoint.rotation);
            Destroy(flash, 0.1f);
        }
    }

    private void ShotgunFire()
    {
        Debug.Log("Shotgun initialised");
        
        float speed = Speed;
        float offset;
        if (applyStrobeOffset)
        {
            offset = Random.Range(0.0F, CalculateStroboscopicOffset(speed));
        }
        else
        {
            offset = 0.0F;
        }

        // Instantiate the tracer graphics
        ShotBullet bullet = Instantiate(bulletPrefab);
        SmokeTrail smokeTrail = Instantiate(smokeTrailPrefab);
        bullet.GetComponent<Bullet>().damage = 18f;
        // Setup callbacks
        bullet.Completed += OnCompleted;
        smokeTrail.Completed += OnCompleted;

        // Use different tracer drawing methods depending on the raycast
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, maxQueryDistance))
        {
            // Since start and end point are known, use DrawLine
            bullet.DrawLine(transform.position, hitInfo.point, speed, offset);
            smokeTrail.DrawLine(transform.position, hitInfo.point, speed, offset);

            // Setup impact callback
            bullet.Arrived += OnImpact;

            void OnImpact(object sender, System.EventArgs e)
            {
                // Handle impact event here
                Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, 0.5F);
            }
        }
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
            int index = Random.Range(0, muzzleFlashPrefabs.Length);
            Quaternion pelletRotation = Quaternion.Euler(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0) * firePoint.rotation;
            GameObject pellet = Instantiate(muzzleFlashPrefabs[index], firePoint.position, pelletRotation);
            
            playerStats.UIHandle.UpdateAllAmmo(playerStats.playerAmmo);
        }

        Debug.Log("Shotgun fired");
    }

    private void OnCompleted(object sender, System.EventArgs e)
    {
        // Handle complete event here
        if (sender is TracerObject tracerObject)
        {
            Destroy(tracerObject.gameObject);
        }
    }

    private float CalculateStroboscopicOffset(float speed) => speed * Time.smoothDeltaTime;
    #endregion

    #region Scriptable object Loading

    public GameObject GunModel;
     public GameObject instGun;
    
     //AnimationClip AnimFire,AnimReload;

    public GameObject Initialize(GunData gunData)
    {
        AudioManager.instance.PlayAudios("Drop Weapon");
        animator.SetBool("isShooting", true);
        GunModel = gunData.gunModel;
        reloadTime = gunData.reloadTime;
        MaxAmmo = gunData.maxAmmo;
        FireRate = gunData.firerate;
        currentGunName = gunData.displayName;
        
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

        CheckAmmo();

       

        return instGun;
    }

    void StartInit(GunData gunData)
    {
        animator.SetBool("isShooting", true);
        GunModel = gunData.gunModel;
        reloadTime = gunData.reloadTime;
        MaxAmmo = gunData.maxAmmo;
        FireRate = gunData.firerate;
        currentGunName = gunData.displayName;

        if (gunData.isCone == true)
        {
            fireMode = FireMode.Cone;
        }
        else { fireMode = FireMode.Projectile; }
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
        CheckAmmo();
    }
    void CheckAmmo()
    {
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
                break;
        }
         playerStats.UIHandle.UpdateAllAmmo(playerStats.playerAmmo);
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


