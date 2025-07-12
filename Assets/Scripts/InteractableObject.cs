using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public GameObject guiPanel; // Reference to the GUI panel
    public TMP_Text interactPromptText; // Reference to the TextMeshPro text for the prompt
    private bool isPlayerNearby = false; // To track if the player is nearby
    private bool isGuiVisible = false; // To track the visibility of the GUI

    public Button interactButton; // Reference to the Button

    AudioManager audioManager; // Reference to the AudioManager for sound effects


    void Start()
    {
        // Ensure the interact prompt text is initially disabled
        if (interactPromptText != null)
        {
            interactPromptText.gameObject.SetActive(false);
        }

        // Ensure the GUI panel is initially disabled
        if (guiPanel != null)
        {
            guiPanel.SetActive(false);
        }

        // Ensure the interact button is initially disabled
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonClicked); // Add listener for button click
        }

        // Find and assign the AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }
    }

    void Update()
    {
        // Check if the player is nearby and the "E" key is pressed
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            audioManager.PlaySFX(audioManager.keyDown); // Play sound effect for key press
            ToggleGUI(); // Toggle the visibility of the GUI panel
        }
    }

    void ToggleGUI()
    {
        isGuiVisible = !isGuiVisible; // Toggle the visibility state
        guiPanel.SetActive(isGuiVisible); // Set the active state of the GUI panel
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // Show the interact prompt text
            if (interactPromptText != null)
            {
                interactPromptText.gameObject.SetActive(true);
            }
            // Show the interact button
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            // Hide the interact prompt text
            if (interactPromptText != null)
            {
                interactPromptText.gameObject.SetActive(false);
            }
            // Hide the interact button
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(false);
            }

            // Optional: Close the GUI when the player leaves the interaction range
            if (isGuiVisible)
            {
                ToggleGUI();
            }
        }
    }

    void OnInteractButtonClicked()
    {
        // Handle button click event
        audioManager.PlaySFX(audioManager.buttonClick);
        ToggleGUI(); // Toggle the visibility of the GUI panel
    }
}
