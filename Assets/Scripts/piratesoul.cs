using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piratesoul : MonoBehaviour
{
    public Transform[] spawnPositions; // Assign 3 positions in the inspector
    public GameObject objectToSpawn;

    void Start(){
        int randomIndex = Random.Range(0, spawnPositions.Length);
        Instantiate(objectToSpawn, spawnPositions[randomIndex].position, Quaternion.identity);
    }
}
