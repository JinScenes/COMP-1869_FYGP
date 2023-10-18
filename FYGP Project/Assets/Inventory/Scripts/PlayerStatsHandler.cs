using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System.Linq;

public class PlayerStatsHandler : MonoBehaviour
{


    public PlayerStats playerStats;
    public Inventory playerInventory;
    public int playerIndex;

    public GameEvent addInven;


    // Awake because some scripts will need this on Start()
    void Awake()
    {
        playerIndex = GetComponent<GamepadInput>().playerIndex;
        playerStats = new PlayerStats(playerIndex);
        playerInventory = playerStats.inventory;
        //print("Got player index" + playerIndex);
        //addInven.Raise(this, player1.inventory);
    }


}
