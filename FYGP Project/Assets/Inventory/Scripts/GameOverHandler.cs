using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    public GameObject deathCanvas;
    public List<GameObject> playerObjs = new List<GameObject>();
    private bool allDied = false;

    // Update is called once per frame
    void Update()
    {


        int dedPlayers = 0;
        foreach (GameObject plr in playerObjs)
        {
            PlayerController plrController = plr.GetComponent<PlayerController>();
            if(plrController.currentHealth <= 0)
            {
                dedPlayers++;
            }
        }

        if(dedPlayers == playerObjs.Count && !allDied && dedPlayers > 0) 
        {
            allDied = true;
            print("QUIT GAME U DIED");
            foreach (GameObject plr in playerObjs)
            {
              Destroy(plr);
            }
            deathCanvas.SetActive(true);
        }

        //print("Dead Players: " + dedPlayers + " TOTAL: " + playerObjs.Count);
    }
}
