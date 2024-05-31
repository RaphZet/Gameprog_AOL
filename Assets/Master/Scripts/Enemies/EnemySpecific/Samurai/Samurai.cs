using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class Samurai : Entity
{
    public Samurai_IdleState idleState { get; private set; }
    public Samurai_MoveState moveState { get; private set; }
    public Samurai_PlayerDetectedState playerDetectedState { get; private set; }
    public Samurai_ChargeState chargeState { get; private set; }
    public Samurai_LookForPlayerState lookForPlayerState { get; private set; }
    public Samurai_MeleeAttackState meleeAttackState { get; private set; }
    public Samurai_StunState stunState { get; private set; }
    public Samurai_DeadState deadState { get; private set; }
    public Samurai_DodgeState dodgeState { get; private set; }
    public Samurai_RangedAttackState rangedAttackState { get; private set; }

    public DamageNumber numberPrefab;
    public GameObject worldPrefab;  // Prefab to instantiate when dead

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    public D_DodgeState dodgeStateData;
    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;

    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangedAttackPosition;

    private Transform aliveTransform; // Transform of the "Alive" child

    // Position offset for damage number
    public Vector3 damageNumberOffset = new Vector3(0, 2, 0);

    public override void Start()
    {
        base.Start();

        moveState = new Samurai_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new Samurai_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new Samurai_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        chargeState = new Samurai_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Samurai_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new Samurai_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new Samurai_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Samurai_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new Samurai_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new Samurai_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

        stateMachine.Initialize(moveState);

        // Find the "Alive" child transform
        aliveTransform = transform.Find("Alive");
        if (aliveTransform == null)
        {
            Debug.LogError("Alive transform not found!");
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);

        if (aliveTransform != null)
        {
            // Draw the offset position for damage number relative to "Alive" transform
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(aliveTransform.position + damageNumberOffset, 0.5f);
        }
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (aliveTransform != null)
        {
            // Calculate the spawn position with offset relative to "Alive" transform
            Vector3 spawnPosition = aliveTransform.position + damageNumberOffset;

            // Show the damage number at the calculated position and make it follow the "Alive" transform
            DamageNumber damageNumber = numberPrefab.Spawn(spawnPosition, attackDetails.damageAmount, aliveTransform);
        }
        else
        {
            Debug.LogError("Alive transform not found!");
        }

        if (isDead)
        {
            // Instantiate the world prefab at the position of the "Alive" transform
            if (worldPrefab != null)
            {
                Instantiate(worldPrefab, aliveTransform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("worldPrefab is not set!");
            }

            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
    }
}