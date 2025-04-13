using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    // Start is called before the first frame update

    void OnCollisionEnter(Collision collision){
        PlayerPrefs.SetInt("end", 1);
        Destroy(gameObject);
    }
}
