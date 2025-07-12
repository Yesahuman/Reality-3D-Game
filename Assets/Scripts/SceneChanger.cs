using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneChanger : MonoBehaviour
{
    // Static stack to store scene history for navigating back to previous scenes
    private static Stack<string> sceneHistory = new Stack<string>();

    // Static instance for implementing Singleton pattern (optional, for making sure only one instance exists)
    private static SceneChanger instance;

    // Reference to PlayerMovement script, used to enable player movement when a scene loads
    PlayerMovement playerMovement;


    // Called when the script instance is being loaded
    private void Awake()
    {
        // Find the PlayerMovement script in the scene and store it in playerMovement.
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Check if PlayerMovement is assigned correctly.
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement is not assigned in the GameManager.");
        }

        // Ensure player movement is enabled when the scene loads
        playerMovement.enabled = true;
    }

    // Method to load a new scene by name.
    public void LoadScene(string sceneName)
    {
        Debug.Log("Attempting to load scene: " + sceneName);

        // Check if the scene can be loaded (i.e., it's added to build settings).
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName); // Load the specified scene.
        }
        else
        {
            // Log an error if the scene cannot be loaded.
            Debug.LogError("Scene " + sceneName + " cannot be loaded. Check if the scene name is correct and added to build settings.");
        }
    }

    // Method to load the previous scene from the scene history stack.
    public void LoadPreviousScene()
    {
        Debug.Log("Attempting to load previous scene.");

        // Check if there is a previous scene in the history stack.
        if (sceneHistory.Count > 0)
        {
            // Pop the last scene name from the stack.
            string previousScene = sceneHistory.Pop();

            // Check if the previous scene can be loaded.
            if (Application.CanStreamedLevelBeLoaded(previousScene))
            {
                SceneManager.LoadScene(previousScene); // Load the previous scene.
            }
            else
            {
                // Log an error if the previous scene cannot be loaded.
                Debug.LogError("Previous scene " + previousScene + " cannot be loaded.");
            }
        }
        else
        {
            // Log an error if there's no previous scene to load.
            Debug.LogError("No previous scene to load.");
        }
    }
}
