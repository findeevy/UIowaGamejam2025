using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    public List<Transform> spawnPoints = new List<Transform>(); // List of spawn points

    public List<Transform> spawnZones = new List<Transform>();

    public List<Texture> oct_textures = new List<Texture>(); // List of textures for the octahedron planes

    private float spawnInterval = 20f; // Time interval between spawns
    private float spawnRadius = 20f; // Radius within which enemies can spawn around a spawn point

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval); 
    }

    
    void SpawnEnemy()
    {
        Debug.Log("SpawnEnemy with interval of " + spawnInterval);

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points available.");
            return;
        }

        // int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform selectedSpawnPoint = spawnPoints[0];

        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0f, // Keep the y-axis unchanged
            Random.Range(-spawnRadius, spawnRadius)
        );

        Vector3 spawnPosition = selectedSpawnPoint.position + randomOffset;

        // Instantiate the enemy at the calculated position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, selectedSpawnPoint.rotation);
        EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        enemyBehavior.textures = oct_textures.ToArray(); // Assign the textures to the enemy
        enemyBehavior.ChangeFace(0);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
