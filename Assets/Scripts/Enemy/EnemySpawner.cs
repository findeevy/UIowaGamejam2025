using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    public List<Transform> spawnPoints = new List<Transform>(); // List of spawn points
    private float spawnInterval = 20f; // Time interval between spawns
    private float spawnRadius = 20f; // Radius within which enemies can spawn around a spawn point

    // Start is called before the first frame update
    void Start()
    {
        AddInitialSP();
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval); 
    }

    void AddInitialSP()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = player.transform.position + player.transform.forward * 70f + player.transform.up * 30f;
            spawnPoint.transform.LookAt(player.transform);

            spawnPoints.Add(spawnPoint.transform);
        }
    }
    
    void SpawnEnemy()
    {
        Debug.Log("SpawnEnemy with interval of " + spawnInterval);

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points available.");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0f, // Keep the y-axis unchanged
            Random.Range(-spawnRadius, spawnRadius)
        );

        Vector3 spawnPosition = selectedSpawnPoint.position + randomOffset;

        // Instantiate the enemy at the calculated position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, selectedSpawnPoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
