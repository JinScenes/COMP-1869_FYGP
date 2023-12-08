using UnityEngine;

public class E_Attack
{
    EnemyFSM enemy;
    public bool attacking;

    public E_Attack(EnemyFSM enemy)
    {
        this.enemy = enemy;
    }

    public void AttackUpdater()
    {
        CheckAttack();
    }

    private void EventAttack()
    {
        Collider[] hitCols = Physics.OverlapSphere(enemy.attackPoint.position, enemy.attackRange, enemy.playerMask);
        Debug.Log($"Attempting attack, found {hitCols.Length} colliders");

        AudioManager.instance.PlayAudios("Zombie Attack");

        foreach (Collider hitCol in hitCols)
        {
            if (enemy.playerObjectNames.Contains(hitCol.transform.gameObject.name))
            {
                hitCol.gameObject.GetComponent<PlayerController>().ApplyDamage(enemy.damage);
            }
        }
    }

    public bool IsPlayerInAttackRange()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(enemy.attackPoint.position, enemy.attackRange, enemy.playerMask);
        return hitPlayers.Length > 0;
    }

    public void CheckAttack()
    {
        if (IsPlayerInAttackRange())
        {
            if (Time.time >= enemy.nextAttackTime)
            {
            attacking = true;
                EventAttack();

                enemy.IsMove = false;
                enemy.lookAtSpeed = 2f;
                enemy.nextAttackTime = Time.time + 1f / enemy.attackRate;
            }
        }
        else
        {
            enemy.IsMove = true;
            attacking = false;
        }
    }   
}
