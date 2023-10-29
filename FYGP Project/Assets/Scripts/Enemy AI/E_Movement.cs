using UnityEngine;

public class E_Movement
{
    EnemyFSM enemy;

    public E_Movement(EnemyFSM enemy)
    {
        this.enemy = enemy;
    }

    public void MovementUpdater()
    {
        MovementFunction();
    }

    private bool IsPlayerInAttackRange()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(enemy.attackPoint.position, enemy.attackRange, enemy.playerMask);
        return hitPlayers.Length > 0;
    }

    public void MovementFunction()
    {
        if (enemy.numberMovement == 0 && enemy.detectionModule.IsPlayerInSightOrDetectionRange() && !IsPlayerInAttackRange())
        {
            Transform closestPlayer = enemy.GetClosestPlayer();
            if (closestPlayer == null)
            {
                return;
            }
            enemy.navMesh.speed = enemy.speed;
            enemy.navMesh.destination = closestPlayer.position;

            if (enemy.timeSinceLastStep >= enemy.minStepInterval)
            {
                enemy.timeSinceLastStep = 0f;
            }

            enemy.timeSinceLastStep += Time.deltaTime;
        }
        else
        {
            enemy.navMesh.speed = 0;
        }
    }
}
