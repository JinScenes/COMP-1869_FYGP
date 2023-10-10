using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Gun", menuName = "Gun Object")  ]
public class GunData : ItemData
{
    // Start is called before the first frame update
    public GameObject gunModel;
    public int maxAmmo;
    public float firerate;
    public AnimationClip reload, fire;
    public Quaternion rotationOffset;
    
    public enum rarity { Common, Uncommon, Rare, Epic , Legendary};
    public enum fireType { Hitscan, Projectile};

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Start()
    {
       
    }
}
