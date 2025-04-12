using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoptoggle : MonoBehaviour
{
    public GameObject gameObject;

    void Start(){
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
       if (Input.GetKeyDown(KeyCode.E)){
            if (PlayerPrefs.GetInt("shop") == 1){
                if (PlayerPrefs.GetInt("pause") == 1){
                    PlayerPrefs.SetInt("pause", 0);
                    gameObject.SetActive(false);
                    Debug.Log("SHIT!");
                }
                else{
                    PlayerPrefs.SetInt("pause", 1);
                    Debug.Log("POOP!");
                    gameObject.SetActive(true);
                }
            }
        }
    }

    public void buySpeed(){
        PlayerPrefs.SetInt("soul", (PlayerPrefs.GetInt("soul") - 20));
        PlayerPrefs.SetInt("speed", (PlayerPrefs.GetInt("speed") + 1));
    }
}
