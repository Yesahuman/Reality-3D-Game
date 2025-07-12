using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    GameManager gameManager; // Reference to the GameManager script
    AudioManager audioManager; // Reference to the AudioManager script

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager using the "GameManager" tag
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!"); // Log an error if the AudioManager is not found
        }
    }

    // Trigger detection when another collider enters the trap's trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Play the trap sound effect
            audioManager.PlaySFX(audioManager.touchTrap);

            // Inflict damage to the player
            gameManager.TakeDamage(10f);

            // Destroy the trap object after triggering
            Destroy(gameObject);
        }
    }
}
