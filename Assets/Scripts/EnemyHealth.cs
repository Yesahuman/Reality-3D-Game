using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.ComponentModel;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy
    public Image healthBarFill; // UI element for health bar fill
    public float attackRange = 5f; // Range within which the enemy will attack
    public float moveSpeed = 3f; // Movement speed of the enemy
    public int damage = 5; // Damage dealt to the player

    private Transform player; // Reference to the player's transform
    private Spawner spawner; // Reference to the spawner script
    private NavMeshAgent navMeshAgent; // NavMeshAgent component for enemy movement
    private Vector3 originalPosition; // Store the original position of the enemy
    private bool isChasing = false; // Flag to check if the enemy is chasing the player

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Set the current health to maximum at the start
        UpdateHealthBar(); // Update the health bar UI


        // // Find and assign the Spawner script in the scene (Initialize Spawner reference)
        spawner = GameObject.FindObjectOfType<Spawner>();
        if (spawner == null)
        {
            Debug.LogError("Spawner reference is null in EnemyHealth script!");
        }

        // Find the player by tag and assign their transform
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player reference is null in EnemyHealth script!");
        }

        // Get the NavMeshAgent component for handling movement
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = moveSpeed; // Set the movement speed of the agent
        }

        else
        {
            Debug.LogError("NavMeshAgent component is missing on the enemy.");
        }

        // Store the original position
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player exists in the scene
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                // Start chasing the player if within attack range
                if (!isChasing)
                {
                    isChasing = true;
                    navMeshAgent.SetDestination(player.position); // Set the destination to the player's position
                }

                // Check if enemy is close enough to attack
                if (distanceToPlayer <= navMeshAgent.stoppingDistance)
                {
                    AttackPlayer(); //Trigger the attack on the player
                }
            }
            else
            {
                // If the player is out of range, stop chasing and return to the original position
                if (isChasing)
                {
                    isChasing = false;
                    StartCoroutine(ReturnToOriginalPosition()); // Start returning to the original position
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce the current health by the damage amount
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method if health reaches zero
        } 
        UpdateHealthBar(); // Update the health bar UI after taking damage
    }

    void UpdateHealthBar()
    {
        // Update the health bar UI element if it's assigned
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth; // Set fill amount based on current health
        }
    }

    void Die()
    {
        Debug.Log("Enemy died"); // Log the enemy's death

        // Notify Spawner that an enemy has been destroyed
        if (spawner != null)
        {
            spawner.OnEnemyDestroyed(gameObject); // Call the method to handle enemy destruction
        }

        Destroy(gameObject); // Destroy the enemy game object
    }

    void AttackPlayer()
    {
        // Attempt to find and damage the player through the GameManager script
        GameManager playerHealth = player.GetComponent<GameManager>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage); // Deal damage to the player
        }
        else
        {
            Debug.LogError("PlayerHealth script is missing on the player.");
        }
    }

    IEnumerator ReturnToOriginalPosition()
    {
        // Set the enemy's destination to their original position
        navMeshAgent.SetDestination(originalPosition);

        // Wait until the enemy has reached the original position
        while (Vector3.Distance(transform.position, originalPosition) > navMeshAgent.stoppingDistance)
        {
            yield return null; // Wait for the next frame
        }

        // Once at the original position, stop moving
        navMeshAgent.ResetPath();
    }
}

