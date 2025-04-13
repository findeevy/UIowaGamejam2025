using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DamagePopUps : MonoBehaviour
{
    public static DamagePopUps instance;

    public GameObject floatingTextPrefab; // Prefab for the floating text

    private Transform player;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (floatingTextPrefab == null)
        {
            Debug.LogError("Floating text prefab is not assigned in the inspector.");
        }
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    public void ChangeLife(int amount)
    {
        Debug.Log("ChangeLife called with amount: " + amount);

        Vector3 randomOffset = 
            player.right * UnityEngine.Random.Range(-1.5f, 1.5f) +
            player.up * 4f +
            player.forward * UnityEngine.Random.Range(4f, 5f);
        Vector3 position = player.position + randomOffset; // Position relative to the player
        Quaternion quaternion = Quaternion.LookRotation(position - player.position); // Look at the player
        // Create a new floating text object

        GameObject floatingText = Instantiate(floatingTextPrefab, position, quaternion);


        String text = amount > 0 ? "+" + amount.ToString() : amount.ToString();
        floatingText.GetComponentInChildren<TextMeshPro>().text = text;

        float t = Mathf.InverseLerp(-100, 100, amount);
        Color c = Color.Lerp(Color.red, Color.green, t);
        floatingText.GetComponentInChildren<TextMeshPro>().color = c;
        
        PlayerPrefs.SetInt("soul", PlayerPrefs.GetInt("soul") + amount);

        // Destroy the floating text after a delay
        Destroy(floatingText, 2f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
