using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float rotationSpeed = 120f;
    
    public GameObject cannonballPrefab;
    public Transform firePoint;

    private Rigidbody rb;
    private float steerDirection = 0f;

    void Awake(){
        rb = GetComponent<Rigidbody>();
        PlayerPrefs.SetInt("pause", 0);
        
        PlayerPrefs.SetInt("soul", 120);
        
        PlayerPrefs.SetInt("speed", 1);
    }

    void Update() {
        //Check for cannon fire.
        if (Input.GetKeyDown(KeyCode.Space)){
            FireCannonball();
        }


        //Steering for wassd.
        steerDirection = 0f;
        if (Input.GetKey(KeyCode.A) && PlayerPrefs.GetInt("pause") == 0){
            steerDirection = -0.3f;
        }
        else if (Input.GetKey(KeyCode.D) && PlayerPrefs.GetInt("pause") == 0) {
            steerDirection = 0.3f;
        }
    }

    void FixedUpdate(){
        //Physics update, only 60 per second.
        HandleMovement();
        HandleSteering();
    }

    void HandleMovement(){
        Vector3 forwardVelocity = Vector3.zero;

        //Check if the player wants to move forward.
        if (Input.GetKey(KeyCode.W) && PlayerPrefs.GetInt("pause") == 0){
            forwardVelocity = transform.forward * moveSpeed;
        }
        //Move the player at the given velocity.
        rb.velocity = forwardVelocity + new Vector3(0, rb.velocity.y, 0);
    }

    void FireCannonball(){
        Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
    }

    void HandleSteering(){
        //Calculate steering angle.
        if (steerDirection != 0f){
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * steerDirection * rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}
