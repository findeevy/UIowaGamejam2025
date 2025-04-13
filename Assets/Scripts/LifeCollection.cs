using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCollection : MonoBehaviour
{
    public float detectionRange = 10f; // Range within which life essences can be detected

    // Update is called once per frame
    void Update()
    {
        DetectAndCollectLifeEssence();
    }

    private void DetectAndCollectLifeEssence()
    {
        // Find all colliders within the detection range
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider collider in colliders)
        {
            // Check if the collider has a LifeEssenceBehavior component
            LifeEssenceBehavior lifeEssence = collider.GetComponent<LifeEssenceBehavior>();
            if (lifeEssence != null && lifeEssence.dangerFactor == 0f) // Check if dangerFactor is 0
            {
                CollectLifeEssence(lifeEssence);
            }
        }
    }

    private void CollectLifeEssence(LifeEssenceBehavior lifeEssence)
    {
        DamagePopUps.instance.ChangeLife(lifeEssence.life); // Update the player's life

        GameObject explosion = Instantiate(lifeEssence.suckPrefab, lifeEssence.transform.position, Quaternion.identity); // Instantiate explosion effect
        explosion.transform.LookAt(transform);

        Destroy(lifeEssence.gameObject); // Destroy the life essence object
        Destroy(explosion, 3f); // Destroy the explosion effect after 5 seconds

    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection range in the editor for visualization
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
