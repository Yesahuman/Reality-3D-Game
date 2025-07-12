using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// Manual Guide script for handling background and description switching
public class Switch : MonoBehaviour
{
    // Array of background game objects to be switched
    public GameObject[] background;

    // Array of description game objects corresponding to backgrounds
    public GameObject[] descriptions;

    // Panel to display the manual guide
    public GameObject manualGuidePanel;

    // Reference to the AudioManager for playing sound effects
    AudioManager audioManager;

    //Index of the currently active background/description
    int index;

    //private bool isManualGuideOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize index to 0
        index = 0;

        // Find and assign the AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }

        // Ensure the manual guide panel starts closed
        manualGuidePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input keys and handle background/description switching
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Play sound effect and switch to the next background/description
            audioManager.PlaySFX(audioManager.keyDown);
            Next();
        }

        // Check for input keys and handle background/description switching
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Play sound effect and switch to the previous background/description
            audioManager.PlaySFX(audioManager.keyDown);
            Previous();
        }

        // Toggle the manual guide panel with the 'G' key
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("G key pressed.");
            GameManager.instance.ToggleManualGuidePanel();
        }

        // Close the manual guide panel with the 'Q' key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            manualGuidePanel.SetActive(false);
        }
    }

    // Method to switch to the next background and description
    public void Next()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);

        // Increment index and loop back to 0 if it exceeds array length
        index++;
        //Check if index exceeds the number of backgrounds
        if (index >= background.Length)
        {
            //If so, loop back to the first background
            index = 0;
        }

        // Update active background and description based on new index

        //Set the active background
        SetActiveBackground();
        //Set the active description
        SetActiveDescription();
    }

    //Method to switch to the previous background & description
    public void Previous()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);

        // Decrement index and loop back to the last element if it goes below 0
        index--;

        //Check if index goes below zero
        if (index < 0)
        {
            //If so, loop back to the last background
            index = background.Length - 1;
        }
        //Set the active background
        SetActiveBackground();
        //Set the active description
        SetActiveDescription();
    }

    // Method to set the active background based on the current index
    void SetActiveBackground()
    {
        //Iterate through all backgrounds
        for (int i = 0; i < background.Length; i++)
        {
            //Activate the background at the current index and deactivate others
            background[i].SetActive(i == index);
        }
    }

    // Method to set the active description based on the current index
    void SetActiveDescription()
    {
        //Iterate through all descriptions
        for (int i = 0; i < descriptions.Length; i++)
        {
            //Activate the description at the current index and deactivate others
            descriptions[i].SetActive(i == index);
        }

    }
}
