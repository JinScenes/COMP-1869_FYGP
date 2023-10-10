using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class PlayerStatsHandler : MonoBehaviour
{


    public PlayerStats player1 = new PlayerStats();
    public PlayerStats player2 = new PlayerStats();
    public PlayerStats player3 = new PlayerStats();
    public PlayerStats player4 = new PlayerStats();

    public static List<PlayerStats> Stats = new List<PlayerStats>();
    public ItemData testItem;
    public int playerIndex;
    Camera cam;


    public GameEvent addInven;


    void Start()

    {

        
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < 4; i++)
        {
            //Stats.Add(new PlayerStats());
        }

        //print("Count is " + Stats.Count);
        for (int i = 0; i < Stats.Count; i++)
        {
            //print(Stats[i]);
        }


        /* player1.inventory.Add(test);
         player1.inventory.Add(test);
         player1.inventory.Add(test);

         player2.inventory.Add(test);
 */
        //player2.inventory.Add(test) ;
        addInven.Raise(this, testItem);
    }

   public void TestCall()
    {
        player1.inventory.Add(testItem);
    }

    private void Update()
    {
       
    }
}
