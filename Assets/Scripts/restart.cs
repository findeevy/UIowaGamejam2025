using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class restart : MonoBehaviour
{
    // Start is called before the first frame update
    public void goBack()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
