using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    // References to the GameManager and AudioManager components
    GameManager gameManager;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager object in the scene and get its GameManager component
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();

        // Log an error if the AudioManager is not found
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Trigger event when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Play the ammo pickup sound effect
            audioManager.PlaySFX(audioManager.touchAmmo);

            // Increase the player's ammo count
            gameManager.AmmoUp(50);

            // Destroy this ammo pickup object
            Destroy(gameObject);
        }
    }
}
