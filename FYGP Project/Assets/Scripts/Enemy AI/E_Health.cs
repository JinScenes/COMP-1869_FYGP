using Unity.VisualScripting;
using UnityEngine;

public class E_Health
{
    EnemyFSM enemy;

    public E_Health(EnemyFSM enemy)
    {
        this.enemy = enemy;
        enemy.maxHealth = enemy.health;
        enemy.healthBarUI.maxValue = 1;
        enemy.healthBarUI.value = Mathf.Clamp01(enemy.health / enemy.maxHealth);
    }

    public void HealthUpdater()
    {
        AIHealth();
        HealthUIFunction();
    }

    private void UpdateHealthBar()
    {
        enemy.healthBarUI.value = Mathf.Clamp01(enemy.health / enemy.maxHealth);
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
        UpdateHealthBar();
        enemy.IsMove = true;
    }

    public bool IsDead()
    {
        return enemy.health <= 0;
    }
}
