using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class PlayerStats : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject deathChunkparticle, deathBloodParticle, DeathAnimation;

    public float currentHealth;

    private GameManager GM;

    // Assign prefab in inspector
    public DamageNumber numberPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = gameObject.GetComponent<Animator>();

        // Set the player animator reference in the Inventory
        Inventory.instance.SetPlayerAnimator(anim);
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        // Spawn damage number UI at player's position with the received damage amount
        DamageNumber damageNumber = numberPrefab.Spawn(transform.position, amount);

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Instantiate death chunk particle
        Instantiate(deathChunkparticle, transform.position, deathChunkparticle.transform.rotation);

        // Get reference to PlayerController to check facing direction
        PlayerController playerController = GetComponent<PlayerController>();

        // Calculate offset based on face direction
        float xOffset = 0f;
        if (playerController.IsFacingRight)
        {
            xOffset = 2f; // Offset to the right
        }
        else
        {
            xOffset = 2f; // Offset to the left
        }

        // Instantiate death blood particle with offset
        Vector3 deathBloodPosition = transform.position + new Vector3(0f, 7.5f, 0f) + new Vector3(xOffset, 0f, 0f);
        Instantiate(deathBloodParticle, deathBloodPosition, deathBloodParticle.transform.rotation);

        // Instantiate death animation
        Instantiate(DeathAnimation, transform.position, DeathAnimation.transform.rotation);

        // Clear inventory on death
        Inventory.instance.ClearInventory();

        // Respawn player
        GM.Respawn();

        // Destroy player object
        Destroy(gameObject);
    }
}
