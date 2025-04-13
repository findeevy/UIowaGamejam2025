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

    public void Initialize(Rigidbody rigidbody, Transform playerTransform)
    {
        rb = rigidbody;
        player = playerTransform;
    }

    public void Patrolling(float deltaTime)
    {
        wanderTimer += deltaTime;
        if (wanderTimer >= wanderDuration)
        {
            PickNewWanderDirection();
        }

        ApplyForce(wanderDirection); // Apply force in the current wander direction
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
        // else{


        //     transform.LookAt(player);
        //     transform.position += directionToPlayer * Time.deltaTime * 10f; 
        // }
        
    }

    public void ApplyForce(Vector3 direction)
    {
        if (rb != null)
        {
            rb.AddForce(direction * pushForce, ForceMode.Impulse);
            wasPushed = true;
            Invoke(nameof(ResetPush), pushInterval); // Reset the push state after 3 seconds
        }
    }

    private void ResetPush()
    {
        wasPushed = false;
    }
}
