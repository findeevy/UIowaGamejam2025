using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody for applying forces
    private Vector3 wanderDirection; // Current direction for wandering
    private float wanderTimer; // Timer for wandering
    private float wanderDuration; // Duration to wander in the current direction
    private Transform player; // Reference to the player

    private float pushForce = 30f; // Force applied to the enemy for movement
    private bool wasPushed = false; // Tracks if the enemy was recently pushed
    private float pushInterval = 3f; // Interval for resetting the push state


    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the enemy object.");
            return;
        }
    }

    public void Patrolling(float deltaTime)
    {
        wanderTimer += deltaTime;
        if (wanderTimer >= wanderDuration)
        {
            PickNewWanderDirection();
        }

        ApplyForce(wanderDirection, 0.3f); // Apply force in the current wander direction
    }

    public void PickNewWanderDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        wanderDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
        wanderDuration = Random.Range(2f, 5f); // Wander for 2 to 5 seconds
        wanderTimer = 0f;
    }

    public void ChasePlayer(float offsetRange)
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 randomAngleOffset = new Vector3(
            Random.Range(-offsetRange, offsetRange),
            Random.Range(0.05f, 0.9f),
            Random.Range(-offsetRange, offsetRange)
        );
        directionToPlayer += randomAngleOffset; // Add a small random offset to the direction
        directionToPlayer.Normalize(); // Normalize the direction vector
        if (!wasPushed){
            ApplyForce(directionToPlayer); // Apply force toward the player
        } 
        
    }

    public void ApplyForce(Vector3 direction, float forceMultiplier = 1f)
    {
        if (rb != null)
        {
            EnemyBehavior enemyBehavior = GetComponent<EnemyBehavior>();
            rb.AddForce(direction * pushForce * forceMultiplier * ((float)enemyBehavior.scale_factor/10), ForceMode.Impulse);
            wasPushed = true;
            Invoke(nameof(ResetPush), pushInterval); // Reset the push state after 3 seconds
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (wasPushed)
            {
                return; // Ignore collisions if the enemy was recently pushed
            } else {
                wasPushed = true; // Set wasPushed to true
                Invoke(nameof(ResetPush), 1f); // Reset wasPushed after 0.5 seconds
            }
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Calculate the direction to push both the enemy and the player
                Vector3 direction = (collision.transform.position - transform.position).normalized;

                // Apply force to the enemy
                rb.AddForce(-direction * 30f, ForceMode.Impulse);

                // Apply force to the player
                playerRb.AddForce(direction * 30f, ForceMode.Impulse);

                // Apply damage to the enemy
                gameObject.GetComponent<EnemyBehavior>().TakeDamage(15, direction);
            }
        }
    }

    private void ResetPush()
    {
        wasPushed = false; // Reset the wasPushed flag
    }
}
