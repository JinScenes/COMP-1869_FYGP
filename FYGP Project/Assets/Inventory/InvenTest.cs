using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenTest : MonoBehaviour
{

    public delegate void OnTest(); // function
    public static OnTest onTest; // reference to it

    public Inventory inventory = new Inventory() ;
    public ItemData itemTest;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory.Add(itemTest);
        inventory.Add(itemTest);
        //onTest += TestEvent;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    void TestEvent()
    {
      /*  print("bla");*/
    }

    // Update is called once per frame
    void Update()
    {
        //onTest?.Invoke();
    }
}
