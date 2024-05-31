using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_PlayerDetectedState : PlayerDetectedState
{

    private Samurai enemy;

    public Samurai_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Samurai enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            if(Time.time >= enemy.dodgeState.startTime + enemy.dodgeStateData.dodgeCooldown)
            {
                stateMachine.ChangeState(enemy.dodgeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }

        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        else if (!isDetectingLedge)
        {
            entity.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
