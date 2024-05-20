using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Crate;

public class LockCrate : MonoBehaviour
{
    //Modify From Chris
    public float launchForce = 5f;
    private GunData selectedGun;
    private GameObject gunPrefab;
    private GameObject gunFloorLootPrefab;
    [SerializeField] private ParticleSystem chestParticleSystem; // Assign in the inspector
    [SerializeField] private float riseHeight = 1.0f; 
    [SerializeField] private float riseDuration = 0.8f;
    [SerializeField] private Vector3 particleSystemMaxScale = new Vector3(1.0f, 1.0f, 1.0f); 
    [SerializeField] private List<GunData> availableGuns;
    [SerializeField] private List<GameObject> availableItems;

    //New Added

    [SerializeField] public int cratePrice = 100; 

    private bool isPlayerNear = false;
    private bool brought = false;
    private GameObject player; 
    private CurrencyHandler currencyHandler;
    //----

    //Base Function
    private void Awake()
    {
        gunFloorLootPrefab = Resources.Load<GameObject>("GunFloorLoot");

        // new
        currencyHandler = FindObjectOfType<CurrencyHandler>();
        if (currencyHandler == null)
        {
            Debug.LogError("CurrencyHandler not found in the scene.");
        }
    }


    private void Update()
    {
        if (isPlayerNear)
        {
            GamepadInput playerInput = player.GetComponent<GamepadInput>();
            if (playerInput != null && playerInput.ButtonNorth)
            {
                TryOpenCrate();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            player = other.gameObject; // Store the player reference
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
            player = null; 
        }
    }

    private void TryOpenCrate()
    {
        if (currencyHandler != null && currencyHandler.totalMoney >= cratePrice && !brought)
        {
            brought = true;
            currencyHandler.AddMoney(-cratePrice);
            StartCoroutine(RiseAndExpand());
        }
        else
        {
            Debug.Log("Not enough money to open the crate.");
        }
    }


    void OpenChest()
    {
        selectedGun = SelectRandomGun(availableGuns);
        GameObject selectedItem = SelectRandomItem(availableItems);

        // Launch the gun to the right
        SpawnAndLaunchGun(selectedGun, true);

        // Launch the item to the left
        if (selectedItem != null)
        {
            SpawnAndLaunchItem(selectedItem, false);
        }

        Destroy(gameObject);
    }

    //Selection

    GunData SelectRandomGun(List<GunData> guns)
    {
        int totalWeight = 0;
        foreach (var gun in guns)
        {
            totalWeight += RarityWeights.Weights[gun.rarity];
        }

        int randomNumber = Random.Range(0, totalWeight);
        foreach (var gun in guns)
        {
            if (randomNumber < RarityWeights.Weights[gun.rarity])
            {
                return gun;
            }
            randomNumber -= RarityWeights.Weights[gun.rarity];
        }

        return null; // Default return if something goes wrong
    }

    GameObject SelectRandomItem(List<GameObject> items)
    {
        if (items.Count == 0) return null;
        int index = Random.Range(0, items.Count);
        return items[index];
    }


    //Opening
    void SpawnAndLaunchGun(GunData gunData, bool toTheRight)
    {
        GameObject gunObject = Instantiate(gunFloorLootPrefab, transform.position, Quaternion.identity);
        ItemCollection itemCollection = gunObject.GetComponent<ItemCollection>();
        if (itemCollection != null)
        {
            itemCollection.item = gunData;
        }
        else
        {
            Debug.LogError("ItemCollection script not found on gun prefab");
        }

        LaunchObject(gunObject, toTheRight);
    }

    void SpawnAndLaunchItem(GameObject itemPrefab, bool toTheRight)
    {
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider cd = item.GetComponent<Collider>();
        rb.isKinematic = false;
        rb.useGravity = true;
        cd.isTrigger = false;
        LaunchObject(item, toTheRight);
    }
    private IEnumerator RiseAndExpand()
    {
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + Vector3.up * riseHeight;

        Vector3 originalScale = chestParticleSystem.transform.localScale;
        Vector3 targetScale = particleSystemMaxScale;

        float elapsedTime = 0;
        while (elapsedTime < riseDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / riseDuration);
            chestParticleSystem.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / riseDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        chestParticleSystem.transform.localScale = targetScale;
        GetComponent<MeshRenderer>().gameObject.SetActive(false);
        // Once the chest has risen and the particle system has expanded, spawn the items
        OpenChest();
    }
    void LaunchObject(GameObject obj, bool toTheRight)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>(); // Add Rigidbody if not already present
        }

        // Determine launch direction based on the 'toTheRight' parameter
        Vector3 launchDirection = toTheRight ? transform.right : -transform.right;
        launchDirection += Vector3.up; // Add some upward force
        launchDirection = launchDirection.normalized;

        rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
    }

}
