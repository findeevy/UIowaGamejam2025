using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    public float moveSpeed = 15f;
    public float rotationSpeed = 120f;

    void Update(){
        HandleMovement();
        HandleSteering();
    }

    void HandleMovement(){
        if (Input.GetKey(KeyCode.W)){
            transform.Translate(
                Vector3.forward * moveSpeed * Time.deltaTime, 
                Space.Self
            );
        }
    }

    void HandleSteering(){
        float steerDirection = 0f;
        if (Input.GetKey(KeyCode.A)){
            steerDirection = -0.3f;
        }
        else if (Input.GetKey(KeyCode.D)){
            steerDirection = 0.3f;
        }

        transform.Rotate(
            Vector3.up * steerDirection * rotationSpeed * Time.deltaTime
        );
    }
}