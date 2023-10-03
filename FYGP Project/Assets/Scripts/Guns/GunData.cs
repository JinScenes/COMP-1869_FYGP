using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Gun", menuName = "Gun Object")  ]
public class GunData : ScriptableObject
{
    // Start is called before the first frame update
    public GameObject gunModel;
    public Sprite gunSprite;
    public float maxAmmo, firerate;
    public AnimationClip reload, fire;
    public enum rarity { Common, Uncommon, Rare, Epic , Legendary};

    // Update is called once per frame
    void Update()
    {
        
    }
}
