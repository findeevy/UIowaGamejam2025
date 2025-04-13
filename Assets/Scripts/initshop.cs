using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class initshop : MonoBehaviour
{
    public Text shoptext;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "pirate"){
        // Debug.Log("Arrive at shop.");
        PlayerPrefs.SetInt("shopz", 1);
        shoptext.text = "Press E for shop!";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "pirate"){
        PlayerPrefs.SetInt("shopz", 0);
        PlayerPrefs.SetInt("pause", 0);
        shoptext.text = "";
        }
    }
}
