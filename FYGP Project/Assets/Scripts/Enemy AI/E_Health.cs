using UnityEngine;

public class E_Health
{
    EnemyFSM enemy;
    public bool hasTakenDamage = false;
    
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

        if (enemy.bloodEffectPrefabs != null && enemy.bloodEffectPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, enemy.bloodEffectPrefabs.Length);
            GameObject bloodEffect = GameObject.Instantiate(enemy.bloodEffectPrefabs[randomIndex],
                enemy.transform.position, Quaternion.identity);

            GameObject.Destroy(bloodEffect, 0.2f);
        }
    }

    public bool IsDead()
    {
        return enemy.health <= 0;
    }
}
