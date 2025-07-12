using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    // Reference to the AudioManager for playing sound effects
    AudioManager audioManager;

    // Initialization method
    private void Start()
    {
        // Find the AudioManager instance in the scene
        audioManager = FindObjectOfType<AudioManager>();

        // Log an error if the AudioManager is not found
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }
    }

    // Method called when a collision occurs with this GameObject
    void OnCollisionEnter(Collision other) 
    {
        // Check if the colliding GameObject has the tag "Player"
        if (other.gameObject.tag.Equals("Player"))
        {
            // Play the sound effect for picking up items
            audioManager.PlaySFX(audioManager.touchPickups);

            // Update the pickup counter in the GameManager
            GameManager.instance.UpdatePickUpCounter(1);

            // Destroy this GameObject
            Destroy(gameObject);
        }
    }
}
