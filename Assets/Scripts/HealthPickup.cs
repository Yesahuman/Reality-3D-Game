using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    GameManager gameManager; // Reference to the GameManager for managing health

    AudioManager audioManager; // Reference to the AudioManager for playing sound effects

    // Start is called before the first frame update
    private void Start()
    {
        // Find the GameManager in the scene using its tag and get its component
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            // Log an error if AudioManager is not found
            Debug.LogError("AudioManager not found!");
        }
    }

    // Called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Play a sound effect when the player collects the health pickup
            audioManager.PlaySFX(audioManager.touchHealth);

            // Increase the player's health through the GameManager
            gameManager.Heal(50f);

            // Destroy this health pickup GameObject
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
