using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    public List<Transform> spawnPoints = new List<Transform>(); // List of spawn points

    public List<Transform> spawnZones = new List<Transform>();

    public List<Texture> oct_textures = new List<Texture>(); // List of textures for the octahedron planes

    public List<Texture> lizard_textures = new List<Texture>(); // List of textures for the cube planes
    public List<Texture> medusa_textures = new List<Texture>(); // List of textures for the sphere planes

    private const int FIRST_SPAWN_POINT = 0;
    private const int VOLCANO_SPAWN_POINT = 1;
    private const int TEMPLE_SPAWN_POINT = 2;

    private const int STARTING_SPAWN_ZONE = 0;
    private const int MAZE_SPAWN_ZONE = 1;  

    private float spawnInterval = 10f; // Time interval between spawns

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        CreateNormalEnemy(spawnPoints[FIRST_SPAWN_POINT].position);
        SpawnBosses();
        spawnInterval = 10f;
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval); 
    }

    
    private void SpawnEnemy()
    {
        Debug.Log("SpawnEnemy with interval of " + spawnInterval);

        if (spawnZones.Count == 0)
        {
            Debug.LogWarning("No spawn points available.");
            return;
        }

        for (int i = 0; i < spawnZones.Count; i++)
        {
            Debug.Log("Checking spawn zone " + i);
            Transform spawnZone = spawnZones[i];
                BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
                if (spawnZoneCollider == null)
                {
                    Debug.LogWarning("Spawn zone does not have a BoxCollider component.");
                    continue;
                }

                if (spawnZoneCollider.bounds.Contains(player.position))
                {
                    Debug.Log("Player is within spawn zone " + i);
                    int scale_factor;
                    if (i == MAZE_SPAWN_ZONE){
                        scale_factor = 3;
                        spawnInterval = 2f;

                    } else {
                        spawnInterval = 10f;
                        scale_factor = 10;
                    }


                    Vector3 randomPoint = new Vector3(
                        Random.Range(spawnZoneCollider.bounds.min.x, spawnZoneCollider.bounds.max.x),
                        spawnZoneCollider.bounds.center.y,
                        Random.Range(spawnZoneCollider.bounds.min.z, spawnZoneCollider.bounds.max.z)
                    );

                    CreateNormalEnemy(randomPoint, scale_factor);
                    
                }

            }
    }

    private void SpawnBosses(){
        GameObject medusa = Instantiate(enemyPrefab, spawnPoints[TEMPLE_SPAWN_POINT].position, Quaternion.identity);
        EnemyBehavior medusaBehavior = medusa.GetComponent<EnemyBehavior>();
        medusaBehavior.Initialize(medusa_textures, 500, 40, 30, 80, 50);

        GameObject lizard = Instantiate(enemyPrefab, spawnPoints[VOLCANO_SPAWN_POINT].position, Quaternion.identity);
        EnemyBehavior lizardBehavior = lizard.GetComponent<EnemyBehavior>();
        lizardBehavior.Initialize(lizard_textures, 300, 30, 20, 80, 50);

    }


    private void CreateNormalEnemy(Vector3 spawnPosition, int scale_factor = 4){
        // Instantiate the enemy at the calculated position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        enemyBehavior.Initialize(oct_textures, 200, 20, scale_factor);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
