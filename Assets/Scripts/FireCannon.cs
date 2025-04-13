using UnityEngine;

public class FireCannon : MonoBehaviour
{
    public GameObject cannonballPrefab; // Prefab for the cannonball
    public Transform firePoint; // Point from which the cannonball is fired

    public void Fire()
    {
        if (cannonballPrefab != null && firePoint != null)
        {
            Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation); // Instantiate the cannonball
        }
        else
        {
            Debug.LogWarning("Cannonball prefab or fire point is not assigned.");
        }
    }
}