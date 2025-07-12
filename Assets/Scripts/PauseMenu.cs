using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Panel displayed when the game is paused
    public GameObject[] otherPanels; // Array to hold references to other panels that should be hidden when the game is paused
    public GameObject testingPanel; // Additional panel to manage

    private bool isPaused = false; // Tracks whether the game is currently paused

    AudioManager audioManager; // Reference to the AudioManager for playing sounds

    PlayerMovement playerMovement; // Reference to the PlayerMovement script

    void Start()
    {
        // Attempt to find the AudioManager and PlayerMovement instances in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }

        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found!");
        }
    }


    void Update()
    {
        // Check for key presses to control game state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioManager.PlaySFX(audioManager.keyDown);

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) // Restart game
        {
            audioManager.PlaySFX(audioManager.keyDown);
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.H)) // Go to main menu
        {
            audioManager.PlaySFX(audioManager.keyDown);
            GoToMainMenu();
        }

        if(Input.GetKeyDown(KeyCode.Q)) // Resume game
        {
            ResumeGame();
        }
    }

    void LockPlayerMovement(bool lockMovement)
    {
        if (playerMovement != null)
        {
            // Assuming you have a way to enable/disable movement in your PlayerMovement script
            playerMovement.enabled = !lockMovement;
        }
    }

    public void PauseGame()
    {
        // Called to pause the game
        audioManager.PlaySFX(audioManager.buttonClick);
        pauseMenuPanel.SetActive(true); // Show the pause menu
        HideOtherPanels(); // Hide other panels
        LockPlayerMovement(true); // Lock player movement
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    public void ResumeGame()
    {
        // Called to resume the game
        audioManager.PlaySFX(audioManager.buttonClick);
        pauseMenuPanel.SetActive(false); // Hide the pause menu
        testingPanel.SetActive(true); // Show testingPanel
        LockPlayerMovement(false); // Unlock player movement

        Time.timeScale = 1f; // Resumes the game
        isPaused = false;
    }

    public void RestartGame()
    {
        // Called to restart the game
        audioManager.PlaySFX(audioManager.buttonClick);
        testingPanel.SetActive(true); // Show testingPanel
        Time.timeScale = 1f; // Resumes the game
        LockPlayerMovement(true); // Lock player movement to reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the current scene
    }

    public void GoToCredits()
    {
        // Called to go to the credits scene
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 1f; // Resumes the game
        LockPlayerMovement(true); // Lock player movement
        SceneManager.LoadScene("Credits"); // Replace "InGame" with your actual main menu scene name
    }

    public void GoToMainMenu()
    {
        // Called to go to the main menu scene
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 1f; // Resumes the game
        LockPlayerMovement(true); // Lock player movement
        SceneManager.LoadScene("School"); // Load main menu scene name

    }

    private void HideOtherPanels()
    {
        // Hides all panels in the otherPanels array
        foreach (GameObject panel in otherPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false); // Hide each panel in the array (change to true if show)
            }
        }
    }
}