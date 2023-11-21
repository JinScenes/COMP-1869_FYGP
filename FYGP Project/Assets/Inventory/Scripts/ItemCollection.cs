using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour, ICollectable
{
    public ItemData item;

    private bool collected;
    private bool equppefull;


    private void Start()
    {
       
    }
    public void Collect(object plrStats)
    {
        if (!collected)
        {
            collected = true;
            PlayerStats playerStats = plrStats as PlayerStats;
            
            if (playerStats.inventory.Add(item))
            {
                Destroy(gameObject);
            } else {
                Debug.Log("Player could not pick up so not removed!");
                collected = false;
            }
            
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.ToString() == "Ground")
        {
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;

        }

    }

}
