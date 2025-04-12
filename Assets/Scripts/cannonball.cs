using UnityEngine;

public class cannonball : MonoBehaviour
{
    public float launchSpeed = 20f;
    private Rigidbody rb;
    public GameObject gameObject;

    void Start(){
        //Launch the ball at a given velocity.
        rb = GetComponent<Rigidbody>();
        Vector3 launchDirection = transform.forward;

        rb.velocity = launchDirection * launchSpeed * PlayerPrefs.GetInt("speed") * 0.5f;
        //Clean up stray balls.
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision){
        //Destroy the cannonball on hit.
         foreach (Transform child in transform)
         {
                Destroy(child.gameObject);
         }
         rb.velocity = Vector3.zero;
         GetComponent<ParticleSystem>().Play();
    }
    void OnCollisionStay(Collision collision){
        //if the ball hits, destroy the particles once they finish rendering.
        if (GetComponent<ParticleSystem>() && !GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }    
}
