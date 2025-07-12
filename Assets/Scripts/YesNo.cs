using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YesNo : MonoBehaviour
{
    // Reference to the panels in the UI
    public GameObject TutorialPanel;
    public GameObject TestingPanel;

    // Reference to the AudioManager for playing sound effects
    AudioManager audioManager;

    private Spawner spawner; // Reference to the Spawner script


    // Start is called before the first frame update
    void Start()
    {
        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!"); // Log an error if not found
        }

        // Find the Spawner script in the scene
        spawner = GameObject.FindObjectOfType<Spawner>();

        // Check if the reference is found
        if (spawner == null)
        {
            Debug.LogError("Spawner script not found in the scene!"); // Log an error if not found
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the Y key is pressed
        if (Input.GetKeyDown(KeyCode.Y)) //Y
        {
            audioManager.PlaySFX(audioManager.keyDown); // Play the keyDown sound effect
            Debug.Log("Y Key pressed");
            print("You'll now visit Tutorial from here!");

            // Hide the TutorialPanel and show the TestingPanel
            TutorialPanel.SetActive(false);
            TestingPanel.SetActive(true);

            // Reload the "Tutorial" scene
            SceneManager.LoadScene("Tutorial");
        }

        //Check if the N key is pressed
        if (Input.GetKeyDown(KeyCode.N)) //N
        {
            audioManager.PlaySFX(audioManager.keyDown); // Play the keyDown sound effect
            Debug.Log("N Key pressed");
            print("You'll now continue from here!");

            // Hide both the TutorialPanel and TestingPanel
            TutorialPanel.SetActive(false);
            TestingPanel.SetActive(false);

            // Trigger enemy spawning
            TriggerSpawning();
        }

        // Add functionality to close panels with the Escape key
        if (Input.GetKeyDown(KeyCode.Q)) // Q key for closing
        {
            audioManager.PlaySFX(audioManager.keyDown); // Play the keyDown sound effect
            Debug.Log("Q Key pressed");

            // Hide the TutorialPanel and show the TestingPanel
            TutorialPanel.SetActive(false);            
            TestingPanel.SetActive(true);

            // Trigger enemy spawning
            TriggerSpawning();
        }
    }

    // Method to start spawning enemies
    public void TriggerSpawning()
    {
        if (spawner != null)
        {
            spawner.StartSpawning(); // Call the StartSpawning method on the spawner
            Debug.Log("Spawning started!");
        }
    }

    // Method to stop spawning enemies
    public void StopSpawning()
    {
        if (spawner != null)
        {
            spawner.StopSpawning(); // Call the StopSpawning method on the spawner
            Debug.Log("Spawning stopped!");
        }
    }
}
