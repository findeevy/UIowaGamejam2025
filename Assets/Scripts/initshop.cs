using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initshop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Arrive at shop.");
        PlayerPrefs.SetInt("shop", 0);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leave shop.");
        PlayerPrefs.SetInt("shop", 1);
    }
}
