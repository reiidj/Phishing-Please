using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class JobIntro : MonoBehaviour
{
    public GameObject EmailPanel; // Assign your panel in the Inspector.
    public GameObject TextPanel; // Temp panel is intro text panel
    public GameObject MainPanel; // Assign your main panel in the Inspector.
    public GameObject OverlayPanel;  // New overlay panel for JobClicked.
    public float displayTime = 2f; // 2 seconds display time

    void Start()
    {
        ShowTemporaryPanel();
    }

    public void ShowTemporaryPanel()
    {
        // Enable the temporary panel
        if (TextPanel != null)
        {
            TextPanel.SetActive(true);
        }

        // Schedule the switch to the main panel after the display time
        Invoke("SwitchToMainPanel", displayTime);
    }

    private void SwitchToMainPanel()
    {
        // Hide the temporary panel
        if (TextPanel != null)
        {
            TextPanel.SetActive(false);
        }

        // Show the main panel
        if (MainPanel != null)
        {
            MainPanel.SetActive(true);
        }
    }

    public void EmailClicked()
    {
        if (EmailPanel != null)
        {
            EmailPanel.SetActive(true); // Show the email panel.
        }

        if (MainPanel != null)
        {
            MainPanel.SetActive(false); // Hide the main panel.
        }
    }

    public void Hide()
    {
        EmailPanel.SetActive(false); // Disable the panel (optional if you need it).
    }

    public void JobClicked()
    {
        if (OverlayPanel != null)
        {
            OverlayPanel.SetActive(true); // Show the overlay panel.
        }

        if (MainPanel != null)
        {
            MainPanel.SetActive(false); // Hide the main panel.
        }
    }


    public void NextButton()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
