using UnityEngine;
[CreateAssetMenu(fileName = "New Gun", menuName = "Gun Object")  ]
public class GunData : ItemData
{
    public GameObject gunModel;
    public int maxAmmo;
    public float firerate;
    public AnimationClip reload, fire;
    public GameObject projectile;
    public rarity rarity;
    public AmmoType type;
    public float reloadTime;
    public bool isCone = false;
    public enum fireType { Hitscan, Projectile};

}
