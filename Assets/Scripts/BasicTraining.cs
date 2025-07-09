using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class BasicTraining : MonoBehaviour
{
    public GameObject IntroPanel;   // text page only
    public GameObject SplashArt;   // Splash art panel.
    public VideoPlayer videoPlayer;   // video to display after splash art.

    public float splashTime = 2f;  // Time to display the splash art.
    public float videoDelay = 4.5f; // Time to wait before showing the video or other elements.

    void Start()
    {
        //pause muna vid otherwise mag start agad i2
        videoPlayer.Pause();

        ShowIntroPanel();
    }

    private void ShowIntroPanel()
    {
        IntroPanel.SetActive(true);  // Show the intro panel.
        Invoke(nameof(ShowSplashArt), splashTime); // Schedule splash art.
    }

    private void ShowSplashArt()
    {
        IntroPanel.SetActive(false); // Hide the intro panel.
        SplashArt.SetActive(true);   // Show the splash art.
        Invoke(nameof(videoDelayFunc), splashTime); 
    }

    private void videoDelayFunc()
    {
        SplashArt.SetActive(false); // Hide the splash art.
        
        //play the video
        videoPlayer.Play();
    }

}
