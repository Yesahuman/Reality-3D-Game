using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // Reference to the Light component representing the flashlight
    public Button toggleButton; // Reference to a UI Button to toggle the flashlight

    private bool isOn = false; // Variable to keep track of the flashlight's state (on or off)

    AudioManager audioManager; // Reference to the AudioManager to play sound effects


    void Start()
    {
        // Check if the flashlight reference is not set, then get the Light component attached to the GameObject
        if (flashlight == null)
        {
            flashlight = GetComponent<Light>();
        }

        // Ensure the flashlight starts in the off state
        flashlight.enabled = isOn;

        // Add a listener to the toggle button to call the ToggleFlashlight method when clicked
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleFlashlight);
        }

        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }
    }

    void Update()
    {
        // Check if the 'F' key is pressed down
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Play the sound effect for key press
            audioManager.PlaySFX(audioManager.keyDown);

            // Toggle the flashlight state
            isOn = !isOn;

            // Enable or disable the flashlight based on the new state
            flashlight.enabled = isOn;
        }
    }

    // Method to toggle the flashlight when the UI button is clicked
    void ToggleFlashlight()
    {
        // Play the sound effect for button click
        audioManager.PlaySFX(audioManager.buttonClick);

        // Toggle the flashlight state
        isOn = !isOn;

        // Enable or disable the flashlight based on the new state
        flashlight.enabled = isOn;
    }
}
