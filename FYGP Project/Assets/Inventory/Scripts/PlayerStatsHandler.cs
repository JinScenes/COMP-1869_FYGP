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
    public ItemData weightItem;
    public gunHolder holder;
    public GameEvent addInven;
    [SerializeField]public GunBase gunBase;

    // Awake because some scripts will need this on Start()
    void Awake()
    {
        GameObject.Find("Extraction").GetComponent<Extraction>().requiredPlayers++;

        playerIndex = GetComponent<GamepadInput>().playerIndex;
        playerStats = new PlayerStats(playerIndex, weightItem,holder, gunBase, gameObject.GetComponent<Consumables>());
        playerInventory = playerStats.inventory;
        //print("Got player index" + playerIndex);
        //addInven.Raise(this, player1.inventory);
    }


}
