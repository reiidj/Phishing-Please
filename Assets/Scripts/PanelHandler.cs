using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelHandler : MonoBehaviour
{

    public GameObject[] emailPanels;  // Array to hold all email panels
    public GameObject mainMenuPanel;

    public GameObject maxShieldPanel;
    public GameObject buyShieldPanel;
    public GameObject noMoneyShieldPanel;
    public GameObject noMoneyHint;

    // Start is called before the first frame update
    void Start()
    {
        // Optionally, initialize the panels (ensure only the main panel is visible initially)
        InitializePanels();

    }


    // Initialize all panels, showing only the main menu panel at the start
    private void InitializePanels()
    {
        mainMenuPanel.SetActive(true);
        // Hide all email panels initially
        foreach (var panel in emailPanels)
        {
            panel.SetActive(false);
        }
    }

    // Show the email panel based on the index provided
    public void ShowEmailPanel(int index)
    {
            // Ensure the index is valid
            if (index >= 0 && index < emailPanels.Length)
            {
                // Hide all panels before showing the selected one
                foreach (var panel in emailPanels)
                {
                    panel.SetActive(false);
                }

                // Show the selected email panel
                emailPanels[index].SetActive(true);
            }
            else
            {
                Debug.LogWarning("Invalid email panel index: " + index);
            }
    }

    // Method to disable a specific email panel by index
    public void DisableEmailPanel(int index)
    {
        // Ensure the index is valid
        if (index >= 0 && index < emailPanels.Length)
        {
            emailPanels[index].SetActive(false); // Disable the email panel
        }
        else
        {
            Debug.LogWarning("Invalid email panel index: " + index);
        }
    }

    // Example method to return to the main menu
    public void BackToMainMenu()
    {
        // Hide all email panels
        foreach (var panel in emailPanels)
        {
            panel.SetActive(false);
        }

        // Show the main menu panel
        mainMenuPanel.SetActive(true);

        // Disable additional panels
        maxShieldPanel.SetActive(false);
        buyShieldPanel.SetActive(false);
        noMoneyShieldPanel.SetActive(false);
        noMoneyHint.SetActive(false);

    }
}
