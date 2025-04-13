using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicset : MonoBehaviour
{
    public int musicIndex;

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "pirate")
        {
            PlayerPrefs.SetInt("music", musicIndex);
        }
    }
}
