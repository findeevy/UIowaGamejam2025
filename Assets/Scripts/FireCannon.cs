using UnityEngine;

public class FireCannon : MonoBehaviour
{
    private bool hasFired = false;
    private float fireInterval = 0.5f;

    public AudioSource shot;

    public GameObject cannonballPrefab; // Prefab for the cannonball
    public Transform firePoint; // Point from which the cannonball is fired

    public void Fire()
    {
        if (hasFired)
            return;

        if (cannonballPrefab != null && firePoint != null && !hasFired)
        {
            
            shot.Play();
            hasFired = true; // Set the flag to prevent multiple firings
            Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation); // Instantiate the cannonball
            Invoke(nameof(ResetFire), fireInterval); // Reset the fire state after the interval
        }
        else
        {
            Debug.LogWarning("Cannonball prefab or fire point is not assigned.");
        }
    }

    private void ResetFire()
    {
        hasFired = false; // Reset the fire state
    }
}