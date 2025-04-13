using UnityEngine;

public class LifeEssenceBehavior : MonoBehaviour
{
    public int life; // Life of the essence, determined by the monster's attack damage
    private const int BASELINE_LIFE = 15; // Baseline life for scale calculation
    private const float DANGER_DECREASE_RATE = 0.25f; // Rate at which danger factor decreases per second

    private float decayAccumulator = 0f;
    public float dangerFactor = 1f; // Initial danger factor (fully dangerous)

    public GameObject explosionPrefab; // Prefab for explosion effect
    public GameObject suckPrefab;

    private float combineRange = 12f;
    private float moveSpeed = 6f;
    public bool isCombining = false; // Flag to indicate if merging is in progress

    private Renderer essenceRenderer; // Renderer to change the color of the essence

    // Method to set life and adjust size
    public void Initialize(int attackDamage, float initialDangerFactor)
    {
        life = attackDamage; // Set life to the monster's attack damage
        dangerFactor = initialDangerFactor; // Set the initial danger factor
        UpdateColor();
        ChangeSize(); // Adjust the size based on the new life value
    }

    private void Awake()
    {
        essenceRenderer = GetComponent<Renderer>(); // Get the Renderer component
        if (essenceRenderer == null)
        {
            Debug.LogError("Renderer component is missing on LifeEssence object.");
        }
    }

    private void ChangeSize()
    {
        float scaleFactor = (float)life / BASELINE_LIFE; // Calculate scale factor
        Vector3 newSize = new Vector3(scaleFactor, scaleFactor, scaleFactor); // Uniform scaling
        Debug.Log("New size: " + newSize);
        transform.localScale = newSize; // Apply the new size
    }

    private void Update()
    {
        UpdateDangerFactor(); // Gradually decrease danger factor
    }

    private void UpdateDangerFactor()
    {


        if (dangerFactor > 0)
        {
            dangerFactor -= DANGER_DECREASE_RATE * Time.deltaTime; // Decrease danger factor over time
            dangerFactor = Mathf.Max(dangerFactor, 0f); // Clamp to 0 to avoid negative values

            if (dangerFactor < 0.1f)
            {
                dangerFactor = 0f; // Set to 0 if it drops below 0.1f
                UpdateColor();
                Debug.Log("Life essence is no longer dangerous.");

            }
            else
            {
                UpdateColor();
            }
       } else {
                // if (Time.deltaTime > Random.Range(0f, 1f)){
                //     life -= 1;
                //     ChangeSize();
                // }
            MergeWithSurroundingLifeEssence();
            MoveTowardsPlayer();

            decayAccumulator += Time.deltaTime;
            if (decayAccumulator >= 0.5f)
            {
                decayAccumulator = 0f;
                life -= 1; // Decrease life over time
                ChangeSize(); // Update size after life change
                if (life <= 0)
                {
                    Destroy(gameObject); // Destroy the essence if life is depleted
                }
            }
       }
        
    }

    private void MoveTowardsPlayer(){
        // Logic to move towards the player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Find the player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized; // Calculate direction to player
        transform.position += directionToPlayer * moveSpeed * Time.deltaTime; // Move towards the player
    }

    private void MergeWithSurroundingLifeEssence(){
        // Logic to merge with surrounding life essence
        Collider[] colliders = Physics.OverlapSphere(transform.position, combineRange); // Check for nearby life essences
        foreach (Collider collider in colliders)
        {
            LifeEssenceBehavior otherEssence = collider.GetComponent<LifeEssenceBehavior>();
            if (otherEssence != null && otherEssence != this
            && otherEssence.dangerFactor == 0f && otherEssence.isCombining == false) // Check if the other essence is not dangerous
            {
                isCombining = true;
                Debug.Log("Merging with another life essence.");
                life += otherEssence.life; // Merge life
                Destroy(otherEssence.gameObject); // Destroy the merged essence
                ChangeSize(); // Update size after merging
            }
        }
    }

    private void UpdateColor()
    {
        if (essenceRenderer != null)
        {

            Color currentColor = dangerFactor == 0f ? Color.green : Color.Lerp(Color.blue, Color.red, (dangerFactor - 0.1f) / 0.9f);
            essenceRenderer.material.color = currentColor; // Change the material color
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (dangerFactor > 0)
            {
                Debug.Log("Life essence collided with player.");
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Create explosion effect
                PlayerPrefs.SetInt("soul", PlayerPrefs.GetInt("soul") - life);

                Destroy(gameObject);
                Destroy(explosion, 2f); // Destroy explosion effect after 2 seconds
            }
        }
    }
}