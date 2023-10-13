using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

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


}
