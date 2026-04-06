using UnityEngine;

public class AudioManager : MonoBehaviour
{ 

    public AudioClip background;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Make sure an AudioClip is assigned in the Inspector
        if (background != null)
        {
            AudioSource.PlayClipAtPoint(background, transform.position); // Play the clip at the GameObject's position
        }
        else
        {
            Debug.LogWarning("Background music clip not assigned in the Inspector.");
        }
    }

    // Add this method to stop and destroy the background music
    public void StopMusic()
    {
        Destroy(gameObject); // Destroy the AudioManager GameObject, which stops the music
    }
}
