using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class Level1 : MonoBehaviour
{

    // Singleton instance
    public static Level1 instance;

    public PanelHandler PanelHandler;

    // game objects
    public GameObject[] safeUnsafePanels; // Safe/Unsafe panels corresponding to each email
    public Button[] sendButtons; // Send buttons for each email
    public Button[] emailButtons;
    bool userMarkedAsSafe;


    //game over view stuff
    public GameObject DayOverviewPanel;  // Panel for day overview
    public GameObject MoneyPanel;       // Panel for money overview

    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI rightDecisionCountText;
    public TextMeshProUGUI wrongDecisionCountText;
    public TextMeshProUGUI addedMoneyText;
    public TextMeshProUGUI minusMoneyText;
    public TextMeshProUGUI totalMoneyText;

    public TextMeshProUGUI rightFlag;
    public TextMeshProUGUI wrongFlag;

    // Values to track for game overview
    private int currentMoney = 0; // money tracking
    private int rightDecisionCount = 0;
    private int wrongDecisionCount = 0;
    private int addedMoney = 0;
    private int minusMoney = 0;

    //HINT STUFF
    public GameObject[] hintPanels;// Array of hint panels
    public GameObject[] hintDecisionPanel;// Array of hint panels
    public Button[] hintButtons; // Array of hint buttons
    public GameObject noMoneyHintPanel; // wala ng money for hint .
    public int hintCost = 0;


    // shield stuff tong nasa baba
    public TextMeshProUGUI[] shieldCountTexts;  // Text to display the shield count on
    public int shieldCount = 3;  // Number of shields remaining / initial shield coutn is 3 
    public int maxShieldCount = 3; // change the maximum shield count if gusto 
    public GameObject shieldPanel; // The panel that contains the Yes and No buttons
    public GameObject maxShieldPanel; // The panel that shows up when max shields are reached
    public GameObject noMoneyShieldPanel; // wala ng money for shield .

    public int shieldCost = 150; // Cost of one shield


    //email marked stuff
    private bool[] isPanelActive = new bool[3]; // Track if the Safe/Unsafe panel is active
    private bool[] isEmailMarked = new bool[3]; // To track if emails are marked
    public GameObject gameOverSplash;  // If shield count is less than 0 show splash, splash image pag naubos na shield

    // Correct answers: true = Safe, false = Unsafe
    private bool[,] correctAnswers = new bool[7, 3]
    {
        { false, false, true },  // Day 1 answers (email 1, 2, 3)
        { true, false, false },  // Day 2 answers (email 1, 2, 3)
        { false, true, true },   // Day 3 answers (email 1, 2, 3)
        { true, false, false },  // Day 4 answers (email 1, 2, 3)
        { false, false, true },  // Day 5 answers (email 1, 2, 3)
        { true, false, false },  // Day 6 answers (email 1, 2, 3)
        { true, false, false }   // Day 7 answers (email 1, 2, 3)
    };


    void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // Destroy the duplicate instance if it exists
            Destroy(gameObject);
        }
        else
        {
            // Set this as the Singleton instance
            instance = this;
            DontDestroyOnLoad(gameObject);  // Don't destroy this object between scene transitions
        }
    }

    void Start()
    {
        Debug.Log("Starting the game.");
        Debug.Log("new scene na to if u see this");

        if (SceneManager.GetActiveScene().buildIndex <= 5) // Check if it's the first scene
        {
            ResetGameData();
            shieldCount = 3;
            currentMoney = 0;

        }
        else
        {
            LoadGameData();
            Debug.Log("Loaded new scene. ShieldCount: " + shieldCount + ", Money: " + currentMoney);
        }

        Debug.Log("Shield Count on Start: " + shieldCount);
        PlayerPrefs.SetInt("Money on start", currentMoney);

        UpdateShieldCount();
        Debug.Log("Starting Scene: " + SceneManager.GetActiveScene().name);

    }


    //DATA STUFF
    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll(); // Clear all saved data
        shieldCount = 3;         // Reset shield count to default
        currentMoney = 0;               // Reset money to default
        Debug.Log("Game data has been reset.");
    }

    // Save game data to PlayerPrefs
    public void SaveGameData()
    {
        PlayerPrefs.SetInt("ShieldCount", shieldCount);
        PlayerPrefs.SetInt("Money", currentMoney);
        PlayerPrefs.Save(); //call Save() to persist the data
    }

    // Load game data from PlayerPrefs
    public void LoadGameData()
    {
        shieldCount = PlayerPrefs.GetInt("ShieldCount", 3);  // Default to 3 if no data is saved
        currentMoney = PlayerPrefs.GetInt("Money", 0);              // Default to 0 if no data is saved
    }

    // Toggle the visibility of the Safe/Unsafe panel by clicking send(letter looking button)
    public void ToggleSafeUnsafePanel(int index)
    {
        // If the Safe/Unsafe panel is already active, close it
        if (isPanelActive[index])
        {
            safeUnsafePanels[index].SetActive(false);
        }
        else
        {
            // Show the Safe/Unsafe panel
            safeUnsafePanels[index].SetActive(true);
        }

        // Toggle the panel's active state
        isPanelActive[index] = !isPanelActive[index];
    }

    // Mark email as Safe function for safe button
    public void MarkAsSafe(int index)
    {
        HandleEmailMarking(index, true);
    }

    // Mark email as Unsafe function for unsafe button
    public void MarkAsUnsafe(int index)
    {
        HandleEmailMarking(index, false);
    }

    private void HandleEmailMarking(int index, bool userMarkedAsSafe)
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex - 5; // Assuming Level 1 has build index 5
        bool isCorrect = correctAnswers[levelIndex, index];

        // Check if the user's choice is correct
        // right answer
        if (userMarkedAsSafe == isCorrect)
        {
            Debug.Log("Correct! Email " + (index + 1) + " marked as " + (userMarkedAsSafe ? "Safe" : "Unsafe") + ".");
            rightDecisionCount++;
        }

        // wrong answer
        else
        {
            Debug.Log("Wrong! Email " + (index + 1) + " was marked incorrectly.");
            shieldCount--;  // Decrement shield count by 1
            wrongDecisionCount++;

        }
        UpdateShieldCount();

        // Disable the email panel and email button 
        PanelHandler.DisableEmailPanel(index);
        emailButtons[index].interactable = false;

        // Mark email as processed
        isEmailMarked[index] = true;

        // Update the proceed button if all emails are marked
        CheckIfAllEmailsMarked();


        // debugging logs here
        Debug.Log("Email " + (index + 1) + " panel is now closed and disabled.");
        Debug.Log("Current Shield: " + shieldCount);
    }

    //SHIELD STUFF
    // Method to update the shield count text across all shieldCountText elements
    private void UpdateShieldCount()
    {

        // Ensure that shieldCount does not exceed the maximum limit
        if (shieldCount > maxShieldCount)
        {
            shieldCount = maxShieldCount;  // If it exceeds, set it to the maximum
        }

        // Loop through each shieldCountText and update its value based on shieldCount
        for (int i = 0; i < shieldCountTexts.Length; i++)
        {
            if (shieldCountTexts[i] != null)
            {
                shieldCountTexts[i].text = shieldCount.ToString(); // Update the text for each shieldCount
            }
        }

        // Optionally, hide the shield count text if no shields are left
        if (shieldCount <= 0)
        {
            foreach (var shieldText in shieldCountTexts)
            {
                shieldText.gameObject.SetActive(false);  // Hide the text if no shields left
            }
            // Show the splash image when shields are exhausted
            gameOverSplash.SetActive(true); // Show the game ove rsplash
        }
        else
        {
            foreach (var shieldText in shieldCountTexts)
            {
                shieldText.gameObject.SetActive(true);  // Show the text if shields are available
            }
        }
    }

    public void OnShieldButtonPressed()
    {
        // Check if the shield count is already at max
        if (shieldCount >= maxShieldCount)
        {
            Debug.Log("Max shield count reached!");
            maxShieldPanel.SetActive(true); // Show the max shield panel
            Invoke("HideMaxShieldPanel", 3f); // Automatically hide the panel after 2 seconds
            return;
        }

        // If shields are not at max, show the Yes/No shield panel
        shieldPanel.SetActive(true);
    }

    // Method called when the No button is pressed
    public void OnNoBuyShield()
    {
        shieldPanel.SetActive(false); // Close the panel
    }

    // Method called when the Yes button is pressed
    public void OnYesBuyShield()
    {
        // Check if the player has enough money to buy a shield
        if (currentMoney >= shieldCost)
        {
            currentMoney -= shieldCost; // Deduct shield cost
            shieldCount++; // Increment shield count
            Debug.Log($"Shield purchased! Shields: {shieldCount}, Money: {currentMoney}");

            // Update shield and money UI
            foreach (var shieldText in shieldCountTexts)
            {
                if (shieldText != null)
                {
                    shieldText.text = $"{shieldCount}";
                }
            }

            if (currentMoneyText != null)
            {
                currentMoneyText.text = $"{currentMoney}";
            }
        }
        else
        {
            noMoneyShieldPanel.SetActive(true); // Show the max shield panel
            Invoke("HideMaxShieldPanel", 3f); // Automatically hide the panel after 2 seconds
            Debug.Log("Not enough money to buy a shield!");
        }

        shieldPanel.SetActive(false); // Close the panel
    }

    // Hide the max shield panel after 2 seconds
    private void HideMaxShieldPanel()
    {
        maxShieldPanel.SetActive(false);
        noMoneyShieldPanel.SetActive(false);
    }


    // Check if all emails are marked
    private void CheckIfAllEmailsMarked()
    {
        bool allEmailsMarked = true;

        for (int i = 0; i < isEmailMarked.Length; i++)
        {
            if (!isEmailMarked[i])
            {
                allEmailsMarked = false;
                Debug.Log("Email " + (i + 1) + " is not marked yet.");
                break;
            }
        }

        Debug.Log("All emails marked: " + allEmailsMarked);

        if (allEmailsMarked && shieldCount > 0)
        {
            Debug.Log("Day overview should be shown");
            ShowDayOverviewPanel(); // Enable the button by activating the GameObject
        }
    }

    public void ShowDayOverviewPanel()
    {
        // Show the DayOverviewPanel and hide the MoneyPanel initially
        DayOverviewPanel.SetActive(true);
        MoneyPanel.SetActive(false);

        rightFlag.text = $"{rightDecisionCount}";//count ng right decisions
        wrongFlag.text = $"{wrongDecisionCount}"; //count ng wrong decisions

        // Schedule the opening of the MoneyPanel after 5 seconds
        Invoke(nameof(OpenMoneyPanel), 5f);
    }

    private void OpenMoneyPanel()
    {
        // Close the DayOverviewPanel and open the MoneyPanel
        DayOverviewPanel.SetActive(false);
        MoneyPanel.SetActive(true);

        // Compute the added and minus money
        addedMoney = rightDecisionCount * 50;  // Example: +50 per correct decision
        minusMoney = wrongDecisionCount * 25; // Example: -25 per wrong decision
        int totalMoney = currentMoney + addedMoney - minusMoney;

        // Update the text UI elements 
        currentMoneyText.text = $"{currentMoney}"; //current money count
        rightDecisionCountText.text = $"{rightDecisionCount}";//count ng right decisions
        wrongDecisionCountText.text = $"{wrongDecisionCount}"; //count ng wrong decisions
        addedMoneyText.text = $"{addedMoney}"; // added money
        minusMoneyText.text = $"{minusMoney}"; // ung money na binawas
        totalMoneyText.text = $"{totalMoney}"; // total money

        // Optionally update the current money for the next round
        currentMoney = totalMoney;
    }

    // HINT BUTTON FUNCTIONALITY
    public void OnHintButtonClicked(int index)
    {

            // Activate the corresponding hint panel
            hintDecisionPanel[index].SetActive(true);
    }

    // Method to handle the "Yes" button click (when the user decides to buy the hint)
    public void OnYesBuyHint(int index)
    {
        if (currentMoney >= hintCost)
        {
            // Deduct the hint cost from the current money
            currentMoney -= hintCost;

            // Activate the hint for the corresponding index
            hintPanels[index].SetActive(true);
            Debug.Log("Hint purchased! Money remaining: " + currentMoney);
        }
        else
        {
            noMoneyHintPanel.SetActive(true); // Show the no money hint panel
            Invoke("HideNoMoneyHintPanel", 2f); // Automatically hide the panel after 2 seconds
            Debug.Log("Not enough money for hint!");
        }

        // Close the hint panel after the user has made their choice
        hintDecisionPanel[index].SetActive(false);
    }

    // Hide the max hint panel after 2 seconds
    private void HideNoMoneyHintPanel()
    {
        noMoneyHintPanel.SetActive(false);
    }

    // Method to handle the "No" button click (when the user cancels the hint purchase)
    public void OnNoBuyHint(int index)
    {
        // Simply close the hint panel without deducting money
        hintDecisionPanel[index].SetActive(false);
        hintPanels[index].SetActive(false);
    }

    //    public Button[] hintButtons;             currentMoney -= hintCost;
    // Method to proceed to the next level
    public void ProceedToNextLevel()
    {
        if (shieldCount >= 0)
        {
            SaveGameData(); // Save game data before transitioning
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.Log("You cannot proceed to the next level, shields are too low.");
        }
    }

    private AudioManager audioManager;

    // Method to be called when the button is clicked
    public void OnGameOverButtonClicked()
    {
        // Reset the values to 0 money and 3 shields
        shieldCount = 3;
        currentMoney = 0;

        // Find the AudioManager in the scene (it should persist across scenes)
        audioManager = FindFirstObjectByType<AudioManager>();

        if (audioManager != null)
        {
            audioManager.StopMusic(); // Stop the music when the game is over
        }
        else
        {
            Debug.LogWarning("AudioManager not found in the scene.");
        }

        SceneManager.LoadScene("MainMenu1");

    }

}
