public class E_Health
{
    EnemyFSM enemy;

    public E_Health(EnemyFSM enemy)
    {
        this.enemy = enemy;
    }

    public void HealthUpdater()
    {
        AIHealth();
        HealthUIFunction();
    }

    private void HealthUIFunction()
    {
        if (enemy.numberHealth == 0 && enemy.health > 0)
        {
            enemy.healthBarUI.gameObject.SetActive(true);
        }
        if (enemy.numberHealth == 1 && enemy.health > 0)
        {
            enemy.healthBarUI.gameObject.SetActive(false);
        }
    }

    private void AIHealth()
    {
        if (enemy.health > 0)
        {
            enemy.attackModule.CheckAttack();
        }
    }


    public void EnemyDamage(float damage)
    {
        enemy.health -= damage;
        enemy.IsMove = true;
    }

    public bool IsDead()
    {
        return enemy.health <= 0;
    }
}
