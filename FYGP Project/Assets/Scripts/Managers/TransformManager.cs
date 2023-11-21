using System.Collections.Generic;
using UnityEngine;
using System;

public class TransformManager : MonoBehaviour
{
    private const int maxPlayers = 4;
    private List<Transform> playerTransforms = new List<Transform>();

    public event Action<Transform> onNewPlayerRegistered;

    public bool RegisterPlayer(GameObject player)
    {
        if (playerTransforms.Count < maxPlayers)
        {
            playerTransforms.Add(player.transform);
            onNewPlayerRegistered?.Invoke(player.transform);

            FadeObjectBlock fadeObjectBlockScript = player.GetComponent<FadeObjectBlock>();

            if (fadeObjectBlockScript)
            {
                fadeObjectBlockScript.AddPlayerToTargets(player.transform);
                return true;
            }
            else
            {
                Debug.LogWarning("FadeObjectBlock script is not found. Has the player spawned yet?");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("Max player count reached!");
            return false;
        }
    }


    public List<Transform> GetAllPlayers()
    {
        return playerTransforms;
    }
}
