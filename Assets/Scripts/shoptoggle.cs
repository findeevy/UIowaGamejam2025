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
            if (PlayerPrefs.GetInt("shopz") == 1){
                if (PlayerPrefs.GetInt("pause") == 1){
                    PlayerPrefs.SetInt("pause", 0);
                    gameObject.SetActive(false);
                }
                else{
                    PlayerPrefs.SetInt("pause", 1);
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
