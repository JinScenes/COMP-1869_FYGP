using UnityEngine;

public class E_Attack
{
    EnemyFSM enemy;

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
        RaycastHit hit;
        float range = 1.3f;
        if (Physics.Raycast(enemy.originPos.position, enemy.originPos.transform.TransformDirection(Vector3.forward), out hit, range))
        {
            if (enemy.playerObjectNames.Contains(hit.transform.gameObject.name))
            {
                hit.transform.gameObject.SendMessage("ApplyDamage", enemy.damage);
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
                EventAttack();

                enemy.IsMove = false;
                enemy.lookAtSpeed = 2f;
                enemy.nextAttackTime = Time.time + 1f / enemy.attackRate;
            }
        }
        else
        {
            enemy.IsMove = true;
        }
    }   
}
