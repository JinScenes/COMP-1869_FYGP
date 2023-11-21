using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmooType : MonoBehaviour
{
    public enum AmmoType { SmallAmmo, MediumAmmo, LargeAmmo };
    [CreateAssetMenu(menuName = "LootAmmo")]
    public class LootAmmo : ScriptableObject
    {
        public AmmoType ammoType;
    }


}
