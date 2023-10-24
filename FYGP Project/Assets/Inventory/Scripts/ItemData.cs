using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create new Item")]
public class ItemData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public bool canStack = true;
    public bool consumable = false;

}
