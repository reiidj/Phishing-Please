using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{   
    private static BackgroundMusic backgroundMusic;

    void Awake()
    {
        if (backgroundMusic == null)
        {
            backgroundMusic = this;
            DontDestroyOnLoad(backgroundMusic);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    // Add this method to stop and destroy the background music
    public void StopMusic()
    {
        Destroy(gameObject); // Destroys the background music GameObject
    }

}
