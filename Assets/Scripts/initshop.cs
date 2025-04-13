using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class initshop : MonoBehaviour
{
    public Text shoptext;
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Arrive at shop.");
        PlayerPrefs.SetInt("shop", 1);
        shoptext.text = "Press E for shop!";
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("Leave shop.");
        PlayerPrefs.SetInt("shop", 0);
        shoptext.text = "";
    }
}
