using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage, attackCooldown;
    [SerializeField]
    private float stunDamageAmount = 1f;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask Damageable;

    private bool gotInput, isAttacking;

    private float lastInputTime = Mathf.NegativeInfinity;
    private float lastAttackTime = Mathf.NegativeInfinity;

    private AttackDetails attackDetails;

    private Animator anim;

    private PlayerController PC;
    private PlayerStats PS;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (combatEnabled && Time.time >= lastAttackTime + attackCooldown) // Periksa apakah cooldown selesai
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                lastAttackTime = Time.time; // Perbarui waktu serangan terakhir
                anim.SetBool("isAttacking", isAttacking);
                audioManager.Play("Electric attack");
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        if (isAttacking) // Serangan hanya dihitung saat sedang menyerang
        {
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, Damageable);

            attackDetails.damageAmount = attack1Damage;
            attackDetails.position = transform.position;
            attackDetails.stunDamageAmount = stunDamageAmount;

            foreach (Collider2D collider in detectedObjects)
            {
                collider.transform.parent.SendMessage("Damage", attackDetails);
            }
        }
    }

    public void FinishAttack1()
    {
        // Reset status serangan
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
    }

    private void Damage(AttackDetails attackDetails)
    {
        if (!PC.GetDashStatus())
        {
            int direction;

            PS.DecreaseHealth(attackDetails.damageAmount);

            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            PC.Knockback(direction);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}