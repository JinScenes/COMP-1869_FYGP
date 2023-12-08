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
    }

    public void UpdateState(EnemyFSM enemyFSM)
    {
        if (enemyFSM.IsMove)
        {
            enemyFSM.animationModule.ChangeState(new WalkingState());
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
        enemyFSM.anim.SetBool("isWalking", true);
        enemyFSM.anim.SetBool("isIdle", false);
    }

    public void UpdateState(EnemyFSM enemyFSM)
    {
        if (!enemyFSM.IsMove)
        {
            enemyFSM.animationModule.ChangeState(new IdleState());
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
        enemyFSM.anim.SetBool("isIdle", true);
        enemyFSM.anim.SetBool("isAttacking", true);
    }

    public void UpdateState(EnemyFSM enemyFSM)
    {
        if (enemyFSM.attackModule.IsPlayerInAttackRange())
        {
            //CONTINUE ATTACKING THE PLAYER
        }
        else
        {
            if (enemyFSM.detectionModule.IsPlayerInSightOrDetectionRange())
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
        //IMPLEMENT LOGIC
    }

    public void ExitState(EnemyFSM enemyFSM)
    {

    }
}

#endregion