using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStatsHandler>().playerStats;
    }
    private void OnCollisionEnter(Collision touched)
    {
        ICollectable collectable = touched.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.Collect(playerStats);
        }

    }

    private void OnTriggerEnter(Collider touched)
    {
        ICollectable collectable = touched.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.Collect(playerStats);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
