using UnityEngine;

public class E_Animation
{
    private EnemyFSM enemyFSM;
    private EnemyState currentState;

    public E_Animation(EnemyFSM enemyFSM)
    {
        this.enemyFSM = enemyFSM;
        ChangeState(new IdleState());
    }

    public void StateUpdater()
    {
        currentState?.UpdateState(enemyFSM);
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.ExitState(enemyFSM);
        currentState = newState;
        currentState?.EnterState(enemyFSM);
    }
}

public interface EnemyState
{
    void EnterState(EnemyFSM enemyFSM);
    void UpdateState(EnemyFSM enemyFSM);
    void ExitState(EnemyFSM enemyFSM);
}

#region Idle

public class IdleState : EnemyState
{
    public void EnterState(EnemyFSM enemyFSM)
    {
        enemyFSM.anim.SetBool("isIdle", true);
        enemyFSM.anim.SetBool("isWalking", false);
        enemyFSM.anim.SetBool("isAttacking", false);
    }

    public void UpdateState(EnemyFSM enemyFSM)
    {
        if (enemyFSM.navMesh.speed != 0)
        {
            enemyFSM.animationModule.ChangeState(new WalkingState());
        }
        else if (enemyFSM.attackModule.attacking == true)
        {
            enemyFSM.animationModule.ChangeState(new AttackingState());
        }
    }

    public void ExitState(EnemyFSM enemyFSM)
    {

    }
}

#endregion

#region Walking

public class WalkingState : EnemyState
{
    public void EnterState(EnemyFSM enemyFSM)
    {
        enemyFSM.anim.SetBool("isIdle", false);
        enemyFSM.anim.SetBool("isWalking", true);
        enemyFSM.anim.SetBool("isAttacking", false);
    }

    public void UpdateState(EnemyFSM enemyFSM)
    {
        if (enemyFSM.navMesh.speed == 0)
        {
            enemyFSM.animationModule.ChangeState(new IdleState());
        }
        else if (enemyFSM.attackModule.attacking == true)
        {
            enemyFSM.animationModule.ChangeState(new AttackingState());
        }
    }


    public void ExitState(EnemyFSM enemyFSM)
    {

    }
}

#endregion

#region Attacking

public class AttackingState : EnemyState
{
    public void EnterState(EnemyFSM enemyFSM)
    {
        enemyFSM.anim.SetBool("isMoving", false);
        enemyFSM.anim.SetBool("isIdle", false);
        enemyFSM.anim.SetBool("isAttacking", true);
    }

    public void UpdateState(EnemyFSM enemyFSM)
    {
        if (enemyFSM.attackModule.attacking == true)
        {
            //CONTINUE ATTACKING THE PLAYER
            enemyFSM.anim.SetBool("isAttacking", true);
        }
        else
        {
            if (enemyFSM.detectionModule.CanSeePlayer())
            {
                enemyFSM.animationModule.ChangeState(new WalkingState());
            }
            else
            {
                enemyFSM.animationModule.ChangeState(new IdleState());
            }
        }
    }

    public void ExitState(EnemyFSM enemyFSM)
    {
        //enemyFSM.anim.SetBool("IsAttacking", false);
    }
}

#endregion

#region Death

public class DeathState : EnemyState
{
    public void EnterState(EnemyFSM enemyFSM)
    {
        enemyFSM.anim.SetTrigger("isDead");
    }

    public void UpdateState(EnemyFSM enemyFSM)
    {
        
    }

    public void ExitState(EnemyFSM enemyFSM)
    {

    }
}

#endregion