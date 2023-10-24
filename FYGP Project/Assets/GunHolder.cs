using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public GunData equippedGun;
    private PlayerController controller;
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(equippedGun != null)
        {
            print(equippedGun);
            //controller.gunRef = equippedGun;
        }
    }
}
