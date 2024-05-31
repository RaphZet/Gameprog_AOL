using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_IdleState : IdleState
{
    private Samurai enemy;
    public Samurai_IdleState(Entity etity, FiniteStateMachine stateMachine, string animBoolName,D_IdleState stateData, Samurai enemy) : base(etity, stateMachine, animBoolName, stateData)
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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
