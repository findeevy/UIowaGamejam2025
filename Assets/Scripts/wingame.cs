using UnityEngine;
using UnityEngine.SceneManagement;

public class wingame : MonoBehaviour
{
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "pirate")
        {
            SceneManager.LoadScene("Win");
        }
    }
}