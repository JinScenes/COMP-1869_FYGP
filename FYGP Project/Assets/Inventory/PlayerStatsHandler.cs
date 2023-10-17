using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System.Linq;

public class PlayerStatsHandler : MonoBehaviour
{


    public PlayerStats playerStats = new PlayerStats();
    public int playerIndex;

    public GameEvent addInven;


    void Start()
    {
        playerIndex = GetComponent<GamepadInput>().playerIndex;
        print("Got player index" + playerIndex);
        //addInven.Raise(this, player1.inventory);
    }

    private void Update()
    {
        //int obj = playerStats.inventory.GetCount();
        //if(obj > 0)
        //{
        //    print(obj);
        //}
    }

}
