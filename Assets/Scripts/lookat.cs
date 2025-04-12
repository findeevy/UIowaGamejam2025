using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookat : MonoBehaviour
{
    public Transform target;
    //Stare at object lol.
    void Update(){
        transform.LookAt(target);
    }

}
