using UnityEngine;

public class cannonball : MonoBehaviour
{
    public float launchSpeed = 20f;
    private Rigidbody rb;
    public GameObject gameObject;

    private Transform ball;
    private int damage = 30;

    void Start(){

        //Launch the ball at a given velocity.
        rb = GetComponent<Rigidbody>();
        Vector3 launchDirection = transform.forward + transform.up * 0.1f;

        rb.velocity = launchDirection * launchSpeed * PlayerPrefs.GetInt("speed") * 0.5f;

        // gameObject.GetComponent<Collider>().enabled = false;
        ball = transform.GetChild(0);
        if (ball != null)
        {
            ball.gameObject.GetComponent<Collider>().enabled = false;
            Invoke(nameof(ReenableCollision), 0.1f); // can't be too quick
        }


        //Clean up stray balls.
        Destroy(gameObject, 5f);

    }
    void ReenableCollision(){
        //Re-enable the collider after a delay.
        if (ball != null)
        {
            ball.gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    void OnCollisionEnter(Collision collision){

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            //If the ball hits an enemy, apply damage.
            EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                Vector3 direction = (collision.transform.position - transform.position).normalized;
                direction.y += 0.5f; // Add some upward force
                direction.Normalize();
                enemy.TakeDamage(damage, direction);
            }

        }


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
