using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class despawn : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if( PlayerPrefs.GetInt("end") == 1){
            Destroy(gameObject);
        }
    }
}
