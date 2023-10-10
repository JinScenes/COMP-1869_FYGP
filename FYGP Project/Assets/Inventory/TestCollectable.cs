using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollectable : MonoBehaviour, ICollectable
{
    public GameObject Collect()
    {
        return gameObject;
    }

    void ICollectable.Collect()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        //ICollectable.OnItemCollected += Collect;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
