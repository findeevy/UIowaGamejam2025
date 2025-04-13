using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private const int MAX_LIFE_BASELINE = 400; // Baseline for maximum health
    private const float DEFAULT_SCALE = 10;
    private const int NORMAL_FACE = 0;
    private const int ANGRY_FACE = 1;
    private const int ATTACK_FACE = 2;

    private Transform player;
    private Renderer enemyRenderer; // Private field to store the Renderer
    public GameObject explosionPrefab;

    public LayerMask whatIsGround, whatIsPlayer; // Layer masks for ground and player

    public GameObject lifeEssencePrefab; // Reference to the projectile prefab

    private float timeBetweenAttacks = 3f; // Default time between attacks
    bool alreadyAttacked;
    private float projectileSpeed = 20f; // Speed of the projectile
    private int attackDamage = 20; // Damage dealt to the player
    private float deathThreshold = 0.15f;

    private int max_life; // Actual maximum health for this enemy
    private int life; // Current health

    private float sightRange = 70f; // Default sight range
    private float attackRange = 35f; // Default attack range
    private bool playerInSightRange, playerInAttackRange;

    private EnemyMovement movement; // Reference to the EnemyMovement module

    private List<Transform> planes;
    public Texture[] textures; // Array of textures for the planes

    private bool isIdle = true;
    private float idleTime = 3f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyRenderer = GetComponent<Renderer>(); // Initialize the Renderer
        Rigidbody rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        // Initialize the EnemyMovement module
        movement = gameObject.AddComponent<EnemyMovement>();
        movement.Initialize(rb, player);
        planes = findDoge();
    }

    private List<Transform> findDoge(){
        List<Transform> tempPlanes = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("doge"))
            {
                tempPlanes.Add(child);
            }
        }
        return tempPlanes;
    }

    private void Start()
    {
        if (explosionPrefab == null)
        {
            Debug.LogError("Explosion prefab is not assigned in the inspector.");
        }

        // Initialize max_life to a random value between 50% and 200% of the baseline
        max_life = Mathf.RoundToInt(MAX_LIFE_BASELINE * Random.Range(0.5f, 2f));
        life = max_life; // Set current life to max_life

        Debug.Log($"Enemy initialized with max_life: {max_life} and current life: {life}");

        movement.PickNewWanderDirection(); // Initialize wandering behavior

        ChangeSize();
        ChangeColor(Color.green); // Change color to green when patrolling
        StartCoroutine(IdleDelay()); // Start the idle delay coroutine
        transform.LookAt(player); // Look at the player
    }

    private void Update()
    {

        if ((float)life / max_life < deathThreshold)
        {
            die();
        }

        if (isIdle){
            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            movement.Patrolling(Time.deltaTime);
            ChangeColor(Color.green); // Change color to green when patrolling
            ChangeFace(NORMAL_FACE);
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            movement.ChasePlayer(0.3f);
            ChangeColor(Color.yellow); // Change color to yellow when chasing the player
        }

        if (playerInAttackRange && playerInSightRange && !alreadyAttacked)
        {
            AttackPlayer();
        }
        ChangeSize(); // Update size based on health
    }

    private IEnumerator IdleDelay(){
        yield return new WaitForSeconds(idleTime); // Wait for 1 second
        isIdle = false; // Set isIdle to false after the delay
    }


    private void die()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Destroy the explosion effect after 2 seconds
        }

        Destroy(gameObject); // Destroy the enemy object
        if (life > 5)
        {
            Vector3 direction = new Vector3(Random.Range(0f, 0.2f), 1, Random.Range(0f, 0.2f));
            GenerateLifeEssence(life, direction);
        }
    }

    private void GenerateLifeEssence(int startingLife, Vector3 direction)
    {
        GameObject essence = Instantiate(lifeEssencePrefab, transform.position, Quaternion.identity); // Instantiate life essence prefab
        essence.GetComponent<LifeEssenceBehavior>().Initialize(startingLife, 0f); // Initialize life essence with the enemy's life

        Rigidbody essenceRb = essence.GetComponent<Rigidbody>();
        if (essenceRb != null)
        {
            essenceRb.AddForce(direction * 25f, ForceMode.Impulse); // Apply upward force to the life essence
        }
    }

    private void ChangeColor(Color color)
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = color; // Change the material color
        }
    }

    private void ChangeFace(int index)
    {
        foreach (Transform plane in planes)
        {
            if (plane != null && index < textures.Length)
            {
                Renderer planeRenderer = plane.GetComponent<Renderer>();
                if (planeRenderer != null)
                {
                    planeRenderer.material.mainTexture = textures[index]; // Change the texture of the plane
                }
            }
        }

    }

    private void ChangeSize()
    {
        Vector3 newSize = new Vector3(
            (float)life / max_life * DEFAULT_SCALE,
            (0.3f + (float)life / max_life / 1.5f) * DEFAULT_SCALE,
            (float)life / max_life * DEFAULT_SCALE
        ); // Calculate new size based on health 
        transform.localScale = newSize; // Change the size of the enemy
    }

    private void AttackPlayer()
    {
        ChangeColor(Color.red); // Change color to red when attacking the player

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            StartCoroutine(AttackFaceChange()); // Change face to attack
            ShootProjectile(); // Launch a projectile
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Reset attack after a delay
        }
    }

    private IEnumerator AttackFaceChange(){
        ChangeFace(ATTACK_FACE); // Change face to attack
        yield return new WaitForSeconds(1f); // Wait for 0.5 seconds
        ChangeFace(ANGRY_FACE); // Change face back to angry
    }

    private void ShootProjectile()
    {
        if (lifeEssencePrefab != null && player != null)
        {
            float attackScaleFactor = Random.Range(0.5f, 2.0f); // Randomize attack scale factor between 0.5 and 2
            int tempAttackDamage = Mathf.RoundToInt(attackDamage * attackScaleFactor);

            Vector3 playerDirection = (player.position - transform.position).normalized;
            Vector3 startPosition = transform.position + playerDirection * 3f; // Start position in front of the enemy

            // Instantiate the projectile at the enemy's position
            GameObject projectile = Instantiate(lifeEssencePrefab, startPosition, Quaternion.identity);

            LifeEssenceBehavior projectileLifeEssence = projectile.GetComponent<LifeEssenceBehavior>();
            projectileLifeEssence.Initialize(tempAttackDamage, 1f); // Set the projectile's life to the enemy's attack damage

            playerDirection = player.transform.position - projectile.transform.position; // Calculate the direction to the player
            playerDirection.Normalize(); // Normalize the direction vector

            // increase by up to 1.5x based on distance to player
            float distanceToPlayer = Vector3.Distance(projectile.transform.position, player.position);
            float distanceFactor = Mathf.Clamp(attackRange / distanceToPlayer * 1.5f, 1f, 1.5f); // Calculate distance factor based on distance to player

            // Get the Rigidbody of the projectile
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(playerDirection * projectileSpeed * attackScaleFactor * distanceFactor, ForceMode.Impulse);
                Debug.Log("Project speed: " + projectileSpeed + "reduction factor: " + ((float)tempAttackDamage / attackDamage));
                life -= tempAttackDamage; // Decrease health when attacking
            }
        }
        else
        {
            Debug.LogWarning("Projectile prefab or player is not assigned");
        }
    }

    public void TakeDamage(int damage, Vector3 direction){
        life -= damage;
        GenerateLifeEssence(damage, direction);

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
