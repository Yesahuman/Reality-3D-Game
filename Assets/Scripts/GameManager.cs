using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Image healthBar; // Reference to the UI Image for the health bar
    public float healthAmount = 100f; // Current health amount

    public TMP_Text healthText; // Reference to the TextMeshPro element displaying health

    public TMP_Text ammoText; // Reference to the TextMeshPro element displaying ammo count
    public int AmmoCount = 100; // Current ammo count

    [Header("--- Pick Ups ---")]
    private int PickUpCurrentCounter = 0; // Counter for current pickups collected
    private int PickUpTotalCounter = 0; // Total pickups in the level
    private GameObject goalGameObject; // Reference to the goal game object
    private TMP_Text PickUpCounterText; // Reference to the TextMeshPro element displaying pickups collected

    [Header("--- UI Panels ---")]
    public GameObject gameTutorialPanel; // Panel for game tutorial
    public GameObject gameStartPanel; // Panel for game start screen
    public GameObject gameOverPanel; // Panel for game over screen
    public GameObject gameWinPanel; // Panel for game win screen
    public GameObject testingPanel; // Panel for testing purposes
    public GameObject creditsPanel; // Panel for credits
    public GameObject manualGuidePanel; // Panel for manual guide

    PlayerMovement playerMovement; // Reference to the PlayerMovement script

    AudioManager audioManager; // Reference to the AudioManager

    private void Awake()
    {
        // Find the PlayerMovement component in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            // Log an error if PlayerMovement is not found
            Debug.LogError("PlayerMovement is not assigned in the GameManager.");
        }

        // Ensure player movement is enabled when the scene loads
        playerMovement.enabled = true;

        // Ensure the game is not paused
        Time.timeScale = 1f;

        // Singleton pattern implementation
        if (instance == null)
        {
            instance = this;
            // Comment out the line below if you want the GameManager to be reset
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this GameManager to ensure only one exists
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize game settings and UI elements
        InitializeGame();

        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            // Log an error if AudioManager is not found
            Debug.LogError("AudioManager not found!");
        }

        testingPanel.SetActive(false);  // Ensure testingPanel starts hidden
    }


    // Method to initialize game settings and UI
    private void InitializeGame()
    {
        // Log a message indicating that game initialization is starting
        Debug.Log("Initializing Game...");

        // Show the start panel
        ShowStartPanel();

        // Find the goal object in the scene using its tag
        goalGameObject = GameObject.FindGameObjectWithTag("Goal");

        // Find and assign the PickUpCounterText UI element
        PickUpCounterText = GameObject.Find("SkillsText").GetComponent<TMP_Text>();

        // Count and assign the total number of pick-up objects in the scene
        PickUpTotalCounter = GameObject.FindGameObjectsWithTag("PickUp").Length;

        // Initially hide the goal object
        goalGameObject.SetActive(false);

        // Initialize the pick-up counter text to 0
        UpdatePickUpCounter(0);

        // Log the status of testingPanel after initialization
        Debug.Log("testPanel status after initialization: " + testingPanel.activeSelf);
        // Ensure that testPanel is not enabled accidentally
        testingPanel.SetActive(false);
    }


    // Restart the game
    public void RestartGame()
    {
        // Log a message indicating that the game is restarting
        Debug.Log("Restarting game...");

        // Reset game data to initial state
        ResetGameData();

        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit the game
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        // The following lines are used to stop play mode in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }


    // Method to handle taking damage
    public void TakeDamage(float damage)
    {
        // Decrease health amount by the damage value
        healthAmount -= damage;

        // Update the health bar based on the new health amount
        healthBar.fillAmount = healthAmount / 100f;

        // Check if health has dropped to zero or below
        if (healthAmount <= 0)
        {
            Die(); // Call the Die method if health is depleted
        }
    }

    // Method to handle player death
    void Die()
    {
        // Log a message indicating the player has died
        Debug.Log("You died.");

        // Disable the player movement script
        playerMovement.enabled = false;

        // Show the mouse cursor and free the mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Play the lose music
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.whenLose);
        }

        // Show the game over panel
        ShowGameOverPanel(); 
    }

    // Method to handle healing
    public void Heal(float healAmount)
    {
        // Increase the health amount by the heal value
        healthAmount += healAmount;

        // Clamp the health amount to ensure it doesn't exceed 100 or fall below 0
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        // Update the health bar based on the new health amount
        healthBar.fillAmount = healthAmount / 100f;
    }

    // Method to increase the player's ammo count
    public void AmmoUp(int ammoup)
    {
        // Add the specified amount of ammo to the current ammo count
        AmmoCount += ammoup;
    }

    // Method to update the pickup counter and check for win condition
    public void UpdatePickUpCounter(int increment)
    {
        // Increase the count and update the UI
        PickUpCurrentCounter += increment;

        // Update the pickup counter text on the UI
        PickUpCounterText.text = "Skills: " + PickUpCurrentCounter + " / " + PickUpTotalCounter;

        // Win Condition, check if the player has collected all pickups
        if (PickUpCurrentCounter == PickUpTotalCounter)
        {
            // Enable Goal GameObject to appear
            goalGameObject.SetActive(true);
        }
    }

    // Method to handle the goal collection
    public void GoalCollected()
    {
        // Stop the game and show the win panel
        gameWinPanel.SetActive(true);

        // Hide the testing panel
        testingPanel.SetActive(false);

        // Play the win music
        if (audioManager != null)
        {
            audioManager.PlayMusic(audioManager.whenWin);
        }

        // Stop player movement
        playerMovement.enabled = false;

        // Show the mouse cursor and free the mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    // Show various panels

    // Show the tutorial panel and hide all other panels
    public void ShowTutorialPanel()
    {
        // Activate the tutorial panel
        gameTutorialPanel.SetActive(true);

        // Deactivate all other panels
        gameStartPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        testingPanel.SetActive(false);
    }

    // Show the start panel and hide all other panels
    public void ShowStartPanel()
    {
        Debug.Log("Showing Start Panel...");
        // Activate the start panel
        gameStartPanel.SetActive(true);

        // Deactivate all other panels
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        testingPanel.SetActive(false);
        gameTutorialPanel.SetActive(false);
    }

    // Show the game over panel and hide all other panels
    public void ShowGameOverPanel()
    {
        // Activate the game over panel
        gameOverPanel.SetActive(true);

        // Deactivate all other panels
        gameTutorialPanel.SetActive(false);
        gameStartPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        testingPanel.SetActive(false);
    }

    // Show the win panel and hide all other panels
    public void ShowWinPanel()
    {
        // Activate the win panel
        gameWinPanel.SetActive(true);

        // Deactivate all other panels
        gameTutorialPanel.SetActive(false);
        gameStartPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        testingPanel.SetActive(false);
    }

    // Show the test panel and hide all other panels
    public void ShowTestPanel()
    {
        // Activate the testing panel
        testingPanel.SetActive(true);

        // Deactivate all other panels
        gameTutorialPanel.SetActive(false);
        gameStartPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
    }


    // Button Methods to Close Panels

    // Close the start panel and show the tutorial panel
    public void CloseStartPanel()
    {
        // Deactivate the start panel
        gameStartPanel.SetActive(false);

        // Activate the tutorial panel
        gameTutorialPanel.SetActive(true);
    }

    // Close the tutorial panel and show the testing panel
    public void CloseTutorialPanel()
    {
        // Deactivate the tutorial panel
        gameTutorialPanel.SetActive(false);

        // Activate the testing panel
        testingPanel.SetActive(true);
    }

    // Close the credits panel
    public void CloseCreditsPanel()
    {
        // Deactivate the credits panel
        creditsPanel.SetActive(false);
    }

    void Update()
    {
        // Restart the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key pressed.");
            RestartGame();
        }

        // Go to home scene and close credits panel
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("H key pressed.");
            CloseCreditsPanel();
            SceneManager.LoadScene("School");
        }

        // Show tutorial panel
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1 key pressed.");
            ShowTutorialPanel();

            if (audioManager != null)
            {
                audioManager.PlayMusic(audioManager.startMusic);
            }
        }

        // Load tutorial scene
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2 key pressed.");
            SceneManager.LoadScene("Tutorial");

            if (audioManager != null)
            {
                audioManager.PlayMusic(audioManager.tutorialMusic);
            }
        }

        // Show credits panel
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3 key pressed.");

            creditsPanel.SetActive(true);

            // Play the credits music
            if (audioManager != null)
            {
                audioManager.PlayMusic(audioManager.creditsMusic);
            }
        }

        // Close credits panel and return to background music
        if (creditsPanel.activeSelf && (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.H)))
        {
            // Deactivate the credits panel
            creditsPanel.SetActive(false);

            // Play the background music again
            if (audioManager != null)
            {
                audioManager.PlayMusic(audioManager.background);
            }
        }

        // Quit the game
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4 key pressed.");
            QuitGame();
        }

        // Update UI text
        ammoText.text = "Ammo Left: " + AmmoCount;
        healthText.text = "Health: " + healthAmount;
        //healthText.text = "" + healthAmount; (alt)
    }

    public void ResetGameData()
    {
        healthAmount = 100f; // Reset health to the maximum value (100)
        AmmoCount = 100; // Reset ammo to the initial count (100)
        PickUpCurrentCounter = 0; // Reset the current pickup counter to 0

        // Update the health bar UI to reflect the reset health
        healthBar.fillAmount = healthAmount / 100f;

        // Update the ammo text UI to reflect the reset ammo count
        ammoText.text = "Ammo Left: " + AmmoCount;

        // Update the pickup counter UI to reflect the reset pickup count
        PickUpCounterText.text = "Skills: " + PickUpCurrentCounter + " / " + PickUpTotalCounter;

        // Hide Goal object if needed
        if (goalGameObject != null)
        {
            goalGameObject.SetActive(false);
        }
    }


    public void ToggleManualGuidePanel()
    {
        // Check if the manual guide panel is assigned
        if (manualGuidePanel != null)
        {
            // Toggle the active state of the manual guide panel
            bool isActive = manualGuidePanel.activeSelf;
            manualGuidePanel.SetActive(!isActive);

            // Show or hide the testing panel based on the manual guide panel's state
            if (testingPanel != null)
            {
                // If the manual guide is active, hide the testing panel
                testingPanel.SetActive(isActive);
            }

            // Debugging log to indicate whether the manual guide was opened or closed
            Debug.Log(isActive ? "Manual guide closed." : "Manual guide opened.");
        }

        else
        {
            // Log an error if the manual guide panel is not assigned in the GameManager
            Debug.LogError("Manual guide panel is not assigned in the GameManager.");
        }
    }
}