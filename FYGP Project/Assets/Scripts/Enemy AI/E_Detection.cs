using UnityEngine;

public class E_Detection
{
    EnemyFSM enemy;

    public E_Detection(EnemyFSM enemy)
    {
        this.enemy = enemy;
    }

    public void DetectionUpdater()
    {
        FetchPlayers();
        CanSeePlayer();
        HearingRange();
        AngleSights();
        WithinRange();
    }

    private bool CanSeePlayer()
    {
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer == null)
        {
            enemy.canSee = false;
            enemy.IsMove = false;
            return false;
        }

        Vector3 fromPos = enemy.originPos.transform.position;
        Vector3 toPos = new Vector3(closestPlayer.position.x, closestPlayer.position.y + 1, closestPlayer.position.z);
        Vector3 dir = toPos - fromPos;

        if (Physics.Raycast(enemy.originPos.position, dir.normalized, out RaycastHit hit, enemy.sightRange))
        {
            enemy.canSee = hit.transform.gameObject.name == enemy.playerObjectName;
            //Debug.Log($"Can see player: {enemy.canSee}");
        }
        else
        {
            enemy.canSee = false;
            //Debug.Log("Raycast did not hit player");
        }
        return enemy.canSee;
    }


    private void HearingRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, enemy.loudness);
        foreach (Collider hitCol in hitColliders)
        {
            if (hitCol.gameObject.name == enemy.playerObjectName)
            {
                enemy.ready = true;
            }
        }
    }

    private void FetchPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        enemy.playerTransforms.Clear();

        foreach (GameObject player in players)
        {
            enemy.playerTransforms.Add(player.transform);
        }
    }

    public Transform GetClosestPlayer()
    {
        float closestDistance = float.MaxValue;
        Transform closestPlayer = null;

        foreach (var playerTransform in enemy.playerTransforms)
        {
            float currentDistance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestPlayer = playerTransform;
            }
        }

        return closestPlayer;
    }

    private void AngleSights()
    {
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer == null) return;

        Vector3 targetDir = closestPlayer.position - enemy.transform.position;
        float angle = Vector3.Angle(targetDir, enemy.transform.forward);

        if (angle < 45f && enemy.disToPlayer < 10f && enemy.canSee)
        {
            enemy.canHit = true;
        }
    }

    private void WithinRange()
    {
        Collider[] playerCheckRange = Physics.OverlapSphere(enemy.transform.position, enemy.detectionRange);
        foreach (Collider player in playerCheckRange)
        {
            if (player.gameObject.name == enemy.playerObjectName && enemy.numberMovement != 0)
            {
                enemy.numberMovement = 0;
                enemy.IsMove = true;
            }
        }
    }

    public bool IsPlayerInSightOrDetectionRange()
    {
        Transform closestPlayer = enemy.GetClosestPlayer();
        if (closestPlayer == null)
        {
            return false;
        }

        float distanceToClosestPlayer = Vector3.Distance(enemy.transform.position, closestPlayer.position);
        return distanceToClosestPlayer <= enemy.sightRange || distanceToClosestPlayer <= enemy.detectionRange;
    }
}
