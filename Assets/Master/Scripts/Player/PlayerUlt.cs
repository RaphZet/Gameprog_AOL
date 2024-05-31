using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUlt : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, ultRadius, ultDamage, ultCooldown, stunDamageAmount = 2f;
    [SerializeField]
    private Transform ultHitboxPos;
    [SerializeField]
    private LayerMask Damageable;

    private bool gotInput, isUsingUltimate;

    private float lastInputTime = Mathf.NegativeInfinity;
    private float lastUltTime = Mathf.NegativeInfinity;

    private AttackDetails attackDetails;

    private Animator anim;

    private PlayerController PC;
    private PlayerStats PS;

    Rigidbody2D rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canUlt", combatEnabled);
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
        if (Input.GetKey(KeyCode.U))
        {
            if (combatEnabled && Time.time >= lastUltTime + ultCooldown) // Periksa apakah cooldown selesai
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
            if (!isUsingUltimate)
            {
                gotInput = false;
                isUsingUltimate = true;
                lastUltTime = Time.time; // Perbarui waktu serangan terakhir
                anim.SetBool("isUsingUltimate", isUsingUltimate);
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void CheckUltHitbox()
    {
        if (isUsingUltimate) // Serangan hanya dihitung saat sedang menyerang
        {
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(ultHitboxPos.position, ultRadius, Damageable);

            attackDetails.damageAmount = ultDamage;
            attackDetails.position = transform.position;
            attackDetails.stunDamageAmount = stunDamageAmount;

            foreach (Collider2D collider in detectedObjects)
            {
                collider.transform.parent.SendMessage("Damage", attackDetails);
            }
        }
    }

    public void StartingUlt()
    {
        // Menonaktifkan pergerakan dan gravitasi
        DisableMovement();
        DisableGravity();
    }



    private void DisableMovement()
    {
        // Mengatur kecepatan berjalan dan melompat menjadi 0
        PC.moveSpeed = 0f;
        PC.JumpPower = 0f;
    }

    private void ResetMovement()
    {
        // Mengatur kecepatan berjalan dan melompat ke kondisi semula
        // Misalnya, jika kecepatan berjalan dan melompat semula adalah 5 dan 10
        PC.moveSpeed = 10f;
        PC.JumpPower = 21f;
    }

    private void DisableGravity()
    {
        // Mengatur gravity scale pada Rigidbody2D menjadi 0
        rb.gravityScale = 0f;
    }

    // Fungsi untuk mengaktifkan kembali gravitasi
    private void EnableGravity()
    {
        // Mengatur gravity scale pada Rigidbody2D menjadi nilai semula
        // Misalnya, jika gravity scale semula adalah 1
        rb.gravityScale = 1f;
    }

    public void FinishUlt()
    {
        // Reset status serangan
        isUsingUltimate = false;
        anim.SetBool("isUsingUltimate", isUsingUltimate);
        ResetMovement();
        EnableGravity();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ultHitboxPos.position, ultRadius);
    }
}
