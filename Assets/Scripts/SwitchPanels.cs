using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles switching between multiple panels in a Unity UI (Tutorial)
public class SwitchPanels : MonoBehaviour
{
    // Array of panels to switch between
    public GameObject[] panels;

    // Specific panel to activate when closing all panels
    public GameObject TestingPanel;

    // Reference to the AudioManager for playing sound effects
    AudioManager audioManager;

    // Index of the currently active panel
    int index;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize index to 0 (first panel)
        index = 0;

        // Find and assign the AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }

        // Set the first panel active
        SetActivePanel();
    }

    // Update is called once per frame
    void Update()
    {
        // Navigate to the next panel with the Right Arrow key
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            audioManager.PlaySFX(audioManager.keyDown);
            Debug.Log("Next");
            NextPanel();
        }

        // Navigate to the previous panel with the Left Arrow key
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            audioManager.PlaySFX(audioManager.keyDown);
            Debug.Log("Previous");
            PreviousPanel();
        }

        // Close all panels and activate the TestingPanel with the Q key
        if (Input.GetKeyDown(KeyCode.Q)) // Q key for closing
        {
            audioManager.PlaySFX(audioManager.keyDown);            
            CloseAllPanels();
            Debug.Log("Q Key pressed");
        }
    }


    // Method to switch to the next panel
    public void NextPanel()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);

        // Increment index
        index++;
        // Check if index exceeds the number of panels
        if (index >= panels.Length)
        {
            // If so, loop back to the first panel
            index = 0;
        }

        // Set the active panel
        SetActivePanel();
    }

    // Method to switch to the previous panel
    public void PreviousPanel()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);

        // Decrement index
        index--;
        // Check if index goes below zero
        if (index < 0)
        {
            // If so, loop back to the last panel
            index = panels.Length - 1;
        }
        // Set the active panel
        SetActivePanel();
    }

    // Method to set the active panel based on the current index
    void SetActivePanel()
    {
        // Iterate through all panels
        for (int i = 0; i < panels.Length; i++)
        {
            // Activate the panel at the current index and deactivate others
            panels[i].SetActive(i == index);
        }
    }

    // Method to close all panels
    public void CloseAllPanels()
    {
        // Deactivate all panels
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // Activate the TestingPanel
        TestingPanel.SetActive(true);
    }
}
