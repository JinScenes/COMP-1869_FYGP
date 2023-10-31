using UnityEngine;

public class E_Health
{
    EnemyFSM enemy;
    bool hasTakenDamage = false;
    
    public E_Health(EnemyFSM enemy)
    {
        this.enemy = enemy;
        enemy.maxHealth = enemy.health;
        enemy.healthBarUI.maxValue = 1;
        enemy.healthBarUI.value = Mathf.Clamp01(enemy.health / enemy.maxHealth);
        enemy.healthBarUI.gameObject.SetActive(false);
    }

    public void HealthUpdater()
    {
        AIHealth();
        HealthUIFunction();
    }

    public void ShowHealthBar()
    {
        enemy.healthBarUI.gameObject.SetActive(true);
    }

    private void HealthUIFunction()
    {
        if (hasTakenDamage)
        {
            enemy.healthBarUI.gameObject.SetActive(true);
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
        enemy.healthBarUI.value = Mathf.Clamp01(enemy.health / enemy.maxHealth);
        hasTakenDamage = true;
        enemy.IsMove = true;
    }

    public bool IsDead()
    {
        return enemy.health <= 0;
    }
}
