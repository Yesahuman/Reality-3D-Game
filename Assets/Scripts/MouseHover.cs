using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Reference to the GameObject that will be displayed or hidden
    public GameObject displayObject;

    // Reference to the AudioManager for playing sound effects
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the displayObject state to inactive at the start
        if (displayObject != null)
        {
            displayObject.SetActive(false);
        }

        else
        {
            Debug.LogError("Display Object is not assigned!"); // Log an error if displayObject is not assigned
        }

        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!"); // Log an error if AudioManager is not found
        }
    }

    // Called when the pointer (mouse) enters the collider area of this GameObject
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Check if the displayObject is assigned
        Debug.Log("Pointer Enter");

        if (displayObject != null)
        {
            // Play the hover sound effect if AudioManager and the sound clip are available
            audioManager.PlaySFX(audioManager.mouseHover);

            // Show the displayObject when the pointer enters
            displayObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("AudioManager or mouseHover clip is missing."); // Log a warning if audioManager or sound clip is missing
        }
    }

    // Called when the pointer (mouse) exits the collider area of this GameObject
    public void OnPointerExit(PointerEventData eventData)
    {
        // Check if the displayObject is assigned
        if (displayObject != null)
        {
            // Hide the displayObject when the pointer exits
            displayObject.SetActive(false);
        }
    }
}
