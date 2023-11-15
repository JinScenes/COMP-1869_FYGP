using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extraction : MonoBehaviour
{
    public GameObject flare;
    // Start is called before the first frame update
    void Start()
    {
        flare.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flare.SetActive(true);
            //Begin Extraction co routine
        }
    }
}
