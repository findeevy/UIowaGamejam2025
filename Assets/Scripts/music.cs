using UnityEngine;
using System.Collections.Generic;

public class music : MonoBehaviour
{
    public List<AudioClip> musicTracks; // Assign your music files in the inspector
    public AudioSource audioSource; // Assign the AudioSource component in the inspector
    public int musicIndex;

    void Start(){
        musicIndex = PlayerPrefs.GetInt("music");
    }

    void Update() {
    if  (musicIndex != PlayerPrefs.GetInt("music")){
        // Safety check
        if (musicIndex < 0 || musicIndex >= musicTracks.Count)
        {
            Debug.LogWarning("Music index out of range. Playing default track.");
            musicIndex = 0;
        }

        if (musicTracks[musicIndex] != null)
        {
            audioSource.clip = musicTracks[musicIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioClip is null at index: " + musicIndex);
        }
    }
    }
}
