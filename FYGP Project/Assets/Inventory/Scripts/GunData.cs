using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun")]
public class GunData : ItemData
{

    public GunData()
    {
        this.canStack = false;
        this.consumable = true;
    }
}
