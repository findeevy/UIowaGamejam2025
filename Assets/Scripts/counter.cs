using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class counter : MonoBehaviour
{
    public Text texty;

    void Start(){
        StartCoroutine(Timing());
    }
    // Update is called once per frame
    void Update()
    {
        var soul = PlayerPrefs.GetInt("soul").ToString();
        var temp = PlayerPrefs.GetInt("speed").ToString();
        texty.text = $"SOUL: {soul}\nSPEED: {temp}";
    }

    IEnumerator Timing()
    {
        while (PlayerPrefs.GetInt("soul") > 0)
        {
            PlayerPrefs.SetInt("soul", (PlayerPrefs.GetInt("soul") - 1));
            yield return new WaitForSeconds(1f);
        }
    }
}
